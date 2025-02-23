using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ClanChat.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="currentUser">Данные пользователя для регистрации</param>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO currentUser)
        {
            var result = await userService.RegisterAsync(currentUser);

            if (result.IsFailure)
            {
                if (result.Error == "Пользователь с таким именем уже существует")
                    return Conflict(new { message = result.Error });

                return StatusCode(500, new { message = result.Error });
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Авторизация и аутентификация пользователя
        /// </summary>
        /// <param name="currentUser">Данные авторизации пользователя</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO currentUser)
        {
            var result = await userService.LoginAsync(currentUser);

            if (result.IsFailure)
            {
                if (result.Error == "Пользователь не найден")
                    return NotFound(new { message = result.Error });

                if (result.Error == "Неверный пароль")
                    return Unauthorized(new { message = result.Error });

                return StatusCode(500, new { message = result.Error });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Изменение клана пользователя
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="clanId">New Clan ID</param>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("changeClan")]
        [ProducesResponseType(typeof(AuthUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeClan([FromQuery] Guid clanId)
        {
            var result = await userService.ChangeClanAsync(clanId);
            if (result.IsFailure)
            {
                if (result.Error == "Пользователь не найден")
                    return NotFound(new { message = result.Error });

                return StatusCode(500, new { message = result.Error });
            }
            return Ok(result.Value);
        }
    }
}
