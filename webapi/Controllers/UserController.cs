using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService service) : Controller
    {
        private readonly UserService service = service;

        [HttpGet]
        public async Task<List<User>> GetUsers() => await service.GetUsersAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<User?> GetUser(string id) => await service.GetUserAsync(id);

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await service.CreateUserAsync(user);
            return Ok(user);
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await service.AuthenticateUserAsync(user.Email, user.Password);

            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { token, user });
        }
    }
}