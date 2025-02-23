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
        /// <param name="newUser">Данные пользователя для регистрации</param>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO newUser)
        {
            var createUserResult = await userService.RegisterAsync(newUser);

            if (createUserResult.IsFailure)
            {
                if (createUserResult.Error == "Пользователь с таким именем уже существует")
                    return Conflict(new { message = createUserResult.Error });

                return StatusCode(500, new { message = createUserResult.Error });
            }

            return Ok(createUserResult.Value);
        }

        /// <summary>
        /// Авторизация и аутентификация пользователя
        /// </summary>
        /// <param name="user">Данные авторизации пользователя</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user)
        {
            var loginUserResult = await userService.LoginAsync(user);
            if (loginUserResult.IsFailure)
            {
                if (loginUserResult.Error == "Пользователь не найден")
                    return NotFound(new { message = loginUserResult.Error });

                if (loginUserResult.Error == "Неверный пароль")
                    return Unauthorized(new { message = loginUserResult.Error });

                return StatusCode(500, new { message = loginUserResult.Error });
            }
            return Ok(loginUserResult.Value);
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
            var changeClanResult = await userService.ChangeClanAsync(clanId);
            if (changeClanResult.IsFailure)
            {
                if (changeClanResult.Error == "Пользователь не найден")
                    return NotFound(new { message = changeClanResult.Error });

                return StatusCode(500, new { message = changeClanResult.Error });
            }
            return Ok(changeClanResult.Value);
        }
    }
}
