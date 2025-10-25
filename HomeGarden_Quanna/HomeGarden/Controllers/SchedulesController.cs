using HomeGarden.Dtos;
using HomeGarden.Dtos.Common;
using HomeGarden.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeGarden.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SchedulesController : BaseApiController
    {
        private readonly HomeGardenDbContext _db;
        public SchedulesController(HomeGardenDbContext db) => _db = db;

        // ======================================================
        // 🔹 GET /api/schedules?plantId=123
        // Danh sách lịch trình của cây (lọc theo quyền)
        // ======================================================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ScheduleListDto>>>> List([FromQuery] long? plantId)
        {
            var query = _db.Schedules.AsNoTracking()
                .Include(s => s.Plant).ThenInclude(p => p.Area)
                .Include(s => s.Status)
                .Where(s => s.IsDeleted == false || s.IsDeleted == null);

            if (plantId.HasValue)
                query = query.Where(s => s.PlantId == plantId.Value);

            if (!User.IsInRole("Admin"))
                query = query.Where(s => s.Plant.Area.UserId == CurrentUserId);

            var list = await query
                .OrderBy(s => s.NextDue)
                .Select(s => new ScheduleListDto
                {
                    ScheduleId = s.ScheduleId,
                    PlantId = s.PlantId,
                    TaskType = s.TaskType,
                    Frequency = s.Frequency,
                    NextDue = s.NextDue,
                    LastDone = s.LastDone,
                    Status = s.Status.Code
                })
                .ToListAsync();

            return ApiResponse.Success(list);
        }

        // ======================================================
        // 🔹 POST /api/schedules
        // Tạo mới lịch cho 1 cây
        // ======================================================
        [HttpPost]
        [Authorize(Roles = "Technician,Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] ScheduleCreateDto dto)
        {
            if (dto.NextDue < DateTime.Now)
                return ApiResponse.Fail<object>("Thời gian NextDue phải lớn hơn hiện tại");

            var plant = await _db.Plants
                .Include(p => p.Area)
                .FirstOrDefaultAsync(p => p.PlantId == dto.PlantId && (p.IsDeleted == false || p.IsDeleted == null));

            if (plant == null)
                return ApiResponse.Fail<object>("Cây không tồn tại");

            if (!User.IsInRole("Admin") && plant.Area.UserId != CurrentUserId)
                return ApiResponse.Fail<object>("Bạn không có quyền tạo lịch cho cây này", 403);

            var pendingId = await _db.StatusDefinitions
                .Where(s => s.Entity == "Schedule" && s.Code == "Pending")
                .Select(s => s.StatusId)
                .FirstOrDefaultAsync();

            var schedule = new Schedule
            {
                PlantId = dto.PlantId,
                TaskType = dto.TaskType.Trim(),
                Frequency = dto.Frequency.Trim(),
                NextDue = dto.NextDue,
                StatusId = pendingId,
                Reminder = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _db.Schedules.Add(schedule);
            await _db.SaveChangesAsync();

            return ApiResponse.Success((object)new { schedule.ScheduleId }, "Tạo lịch thành công");
        }

        // ======================================================
        // 🔹 POST /api/schedules/{id}/done
        // Đánh dấu lịch đã hoàn thành và sinh lịch mới kế tiếp
        // ======================================================
        [HttpPost("{id:long}/done")]
        public async Task<ActionResult<ApiResponse<string>>> MarkDone(long id, [FromBody] ScheduleDoneDto dto)
        {
            var schedule = await _db.Schedules
                .Include(x => x.Plant).ThenInclude(p => p.Area)
                .FirstOrDefaultAsync(x => x.ScheduleId == id && (x.IsDeleted == false || x.IsDeleted == null));

            if (schedule == null)
                return ApiResponse.Fail<string>("Lịch không tồn tại");

            if (!User.IsInRole("Admin") && schedule.Plant.Area.UserId != CurrentUserId)
                return ApiResponse.Fail<string>("Bạn không có quyền thao tác lịch này", 403);

            // ✅ Cập nhật trạng thái Completed
            var completedId = await _db.StatusDefinitions
                .Where(x => x.Entity == "Schedule" && x.Code == "Completed")
                .Select(x => x.StatusId)
                .FirstOrDefaultAsync();

            schedule.LastDone = dto.DoneAt;
            schedule.StatusId = completedId;
            schedule.UpdatedAt = DateTime.Now;

            // ✅ Tạo lịch Pending kế tiếp
            var pendingId = await _db.StatusDefinitions
                .Where(x => x.Entity == "Schedule" && x.Code == "Pending")
                .Select(x => x.StatusId)
                .FirstOrDefaultAsync();

            var nextDue = ComputeNextDue(dto.DoneAt, schedule.Frequency);

            var nextSchedule = new Schedule
            {
                PlantId = schedule.PlantId,
                TaskType = schedule.TaskType,
                Frequency = schedule.Frequency,
                NextDue = nextDue,
                StatusId = pendingId,
                Reminder = schedule.Reminder,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _db.Schedules.Add(nextSchedule);
            await _db.SaveChangesAsync();

            return ApiResponse.Success("Đã đánh dấu hoàn thành và tạo lịch kế tiếp");
        }

        // ======================================================
        // 🔸 Logic tính NextDue kế tiếp
        // ======================================================
        private DateTime ComputeNextDue(DateTime from, string frequency)
        {
            frequency = frequency?.ToLower() ?? "daily";

            if (frequency == "daily") return from.AddDays(1);
            if (frequency == "weekly") return from.AddDays(7);
            if (frequency == "monthly") return from.AddMonths(1);

            // hỗ trợ custom như "every3days"
            if (frequency.StartsWith("every"))
            {
                var num = new string(frequency.Where(char.IsDigit).ToArray());
                if (int.TryParse(num, out var n) && n > 0)
                    return from.AddDays(n);
            }

            return from.AddDays(1);
        }
    }
}
