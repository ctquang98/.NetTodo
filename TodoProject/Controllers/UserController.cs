using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoProject.Helpers;
using TodoProject.Models.DTOs;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly JwtHelper jwtHelper;

        public UserController(IConfiguration config, UserService userService)
        {
            this.userService = userService;
            var jwtSettings = config.GetSection("Jwt");
            jwtHelper = new JwtHelper(jwtSettings["Key"], jwtSettings["Issuer"], jwtSettings["Audience"]);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            var user = await userService.Login(body);
            if (user == null) return Unauthorized("Invalid username or password.");
            return Ok(new { token = $"bearer {jwtHelper.generateToken(user)}" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest body)
        {
            var user = await userService.Register(body);
            if (user == null) return Ok("Invalid");
            return Ok(new { token = $"bearer {jwtHelper.generateToken(user)}" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await userService.GetAll());
        }
    }
}
