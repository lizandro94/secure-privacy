using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services;

namespace webapi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController(UserService service) : Controller
    {
        private readonly UserService service = service;

        [HttpGet]
        public async Task<List<User>> GetUsers() => await service.GetUsersAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<User?> GetUser(string id) => await service.GetUserAsync(id);

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await service.CreateUserAsync(user);
            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await service.DeleteUserAsync(id);
            return Ok();
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var (token, loggedInuser) = await service.AuthenticateUserAsync(user.UserName, user.Password);

            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new
            {
                token,
                id = loggedInuser?.Id,
                firstName = loggedInuser?.FirstName,
                lastName = loggedInuser?.LastName,
                username = loggedInuser?.UserName
            });
        }
    }
}