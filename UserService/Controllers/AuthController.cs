using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;
using System.Text.Json;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userService.Authenticate(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Stub token: JSON string with username and role
            var payload = JsonSerializer.Serialize(new { Username = user.Username, Role = user.Role.ToString() });
            var token = $"Bearer {payload}";
            return Ok(new { token });
        }
    }
}
