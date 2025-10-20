using HomeGarden.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeGarden.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly HomeGardenDbContext _context;


        public CustomerController(HomeGardenDbContext homeGardenDb)
        {
            _context = homeGardenDb;
        }

        [HttpGet("Dashboard")]
        public IActionResult GetAll()
        {
            var data = _context.Users.ToList();

            return Ok(data);

        }

    }
}
