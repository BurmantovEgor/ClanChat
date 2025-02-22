using ClanChat.Abstractions.Message;
using ClanChat.Core.DTOs.Message;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClanChat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/message")]
    public class MessageController(IMessageService messageService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDTO newMsg)
        {
            var result = await messageService.SendMessage(newMsg);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok();
        }

        [HttpGet("lastMessages/{count}")]
        public async Task<IActionResult> LastMessages(int count)
        {
            var result = await messageService.GetLastMessages(count);
            return Ok(result.Value);
        }
    }
}
