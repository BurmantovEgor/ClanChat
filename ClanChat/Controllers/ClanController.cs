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
        [ProducesResponseType((typeof(List<ClanDTO>)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AllClans()
        {
            var getClansResult = await clanService.GetAllAsync();
            if (getClansResult.IsFailure) return NotFound(new { message = getClansResult.Error });

            return Ok(getClansResult.Value);
        }

        /// <summary>
        /// Получить информацию о клане по его ID.
        /// </summary>
        /// <param name="clanId">Идентификатор клана.</param>
        [HttpGet("id/{clanId}")]
        [ProducesResponseType((typeof(ClanDTO)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ClanById(Guid clanId)
        {
            var findClanResult = await clanService.FindByIdAsync(clanId);
            if (findClanResult.IsFailure) return NotFound(new { message = findClanResult.Error });

            return Ok(findClanResult.Value);
        }

        /// <summary>
        /// Создать новый клан.
        /// </summary>
        /// <param name="newClan">Данные нового клана.</param>
        [HttpPost]
        [ProducesResponseType((typeof(ClanDTO)), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NewClan([FromBody] CreateClanDTO newClan)
        {
            var createClanResult = await clanService.CreateNewAsync(newClan);
            if (createClanResult.IsFailure)
            {
                if (createClanResult.Error == "Clan is already exists")
                    return Conflict(new { message = createClanResult.Error });

                return StatusCode(500, new { message = createClanResult.Error });
            }
            return Created(nameof(ClanById), createClanResult.Value);
        }
    }
}
