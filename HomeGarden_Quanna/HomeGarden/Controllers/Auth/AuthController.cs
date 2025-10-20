using HomeGarden.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeGarden.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HomeGardenDbContext _context;

        public AuthController(HomeGardenDbContext homeGardenDbContext)
        {
            _context = homeGardenDbContext;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return Ok();
        }

    }
}
