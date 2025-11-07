using HomeGarden.Dtos;
using HomeGarden.Dtos.Common;
using HomeGarden.Models;
using HomeGarden.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeGarden.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EmailsController : BaseApiController
    {
        private readonly HomeGardenDbContext _db;
        private readonly EmailService _emailService;

        public EmailsController(HomeGardenDbContext db, EmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        // 🔹 GET /api/emails/templates
        [HttpGet("templates")]
        public async Task<ActionResult<ApiResponse<List<EmailTemplateDto>>>> GetTemplates()
        {
            var templates = await _db.EmailTemplates
                .OrderBy(t => t.Code)
                .Select(t => new EmailTemplateDto
                {
                    TemplateId = t.TemplateId,
                    Code = t.Code,
                    Subject = t.Subject,
                    Description = t.Description,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return ApiResponse.Success(templates);
        }

        // 🔹 POST /api/emails/send
        [HttpPost("send")]
        public async Task<ActionResult<ApiResponse<string>>> SendEmail([FromBody] EmailSendDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null)
                return ApiResponse.Fail<string>("Người dùng không tồn tại");

            try
            {
                // Gửi mail thật qua Gmail SMTP
                await _emailService.SendAsync(user.Email, dto.Subject, dto.Content);

                // Lưu log vào bảng email_notifications
                var email = new EmailNotification
                {
                    UserId = dto.UserId,
                    Subject = dto.Subject,
                    Content = dto.Content,
                    Sent = true,
                    SendTime = DateTime.Now,
                    SentAt = DateTime.Now
                };
                _db.EmailNotifications.Add(email);
                await _db.SaveChangesAsync();

                return ApiResponse.Success("Gửi email thành công ✅");
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail<string>($"Lỗi khi gửi mail: {ex.Message}");
            }
        }
    }
}
