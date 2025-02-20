using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClanChat.Controllers
{
    [ApiController]
    [Route("clan")]
    public class ClanController(IClanService clanService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> AllClans()
        {
            var result = await clanService.GetAll();
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> NewClan([FromBody] ClanDTO newClan)
        {
            var result = await clanService.CreateNew(newClan);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
