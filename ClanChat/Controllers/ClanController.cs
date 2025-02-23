using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs.Clan;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ClanChat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/clan")]
    public class ClanController(IClanService clanService) : ControllerBase
    {
        /// <summary>
        /// Получить список всех кланов.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((typeof(List<ClanDTO>)),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AllClans()
        {
            var result = await clanService.GetAllAsync();
            if (result.IsFailure) return NotFound(new { message = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Получить информацию о клане по его ID.
        /// </summary>
        /// <param name="clanId">Идентификатор клана.</param>
        [HttpGet("id/{clanId}")]
        [ProducesResponseType((typeof(ClanDTO)),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ClanById(Guid clanId)
        {
            var result = await clanService.FindByIdAsync(clanId);
            if (result.IsFailure) return NotFound(new { message = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Создать новый клан.
        /// </summary>
        /// <param name="newClan">Данные нового клана.</param>
        [HttpPost]
        [ProducesResponseType((typeof(ClanDTO)),StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NewClan([FromBody] CreateClanDTO newClan)
        {
                       var result = await clanService.CreateNewAsync(newClan);

            if (result.IsFailure)
            {
                if (result.Error == "Clan is already exists")
                    return Conflict(new { message = result.Error });

                return StatusCode(500, new { message = result.Error });
            }

            return Created(nameof(ClanById), result.Value);
        }
    }
}
