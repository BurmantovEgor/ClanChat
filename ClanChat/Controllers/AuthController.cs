using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace ClanChat.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO currentUser)
        {
            var result = await userService.Register(currentUser);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO currentUser)
        {
            var result = await userService.Login(currentUser);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
