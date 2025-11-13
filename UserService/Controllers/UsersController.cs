using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            return Ok(_userService.ListUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] User user)
        {
            var created = _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var updated = _userService.UpdateUser(id, user);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            // Only Admin can delete
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role != UserRole.Admin.ToString())
                return Forbid("Only Admin can delete users.");

            var deleted = _userService.DeleteUser(id);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
