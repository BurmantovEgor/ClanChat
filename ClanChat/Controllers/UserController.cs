using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClanChat.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO currentUser)
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

        [HttpPut("changeClan")]
        public async Task<IActionResult> ChangeClan(Guid userId, Guid clanId)
        {
            var result = await userService.ChangeClan(userId, clanId);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
