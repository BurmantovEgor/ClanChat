﻿using ClanChat.Abstractions.Message;
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
        /*/// <summary>
        /// Отправить сообщение в клановый чат.
        /// </summary>
        /// <param name="newMsg">Данные нового сообщения</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDTO newMsg)
        {
           *//* var senMessageResult = await messageService.SendMessageAsync(newMsg);
            if (senMessageResult.IsFailure) return BadRequest(new { message = senMessageResult.Error });*//*
            return Ok(new { message = "Сообщение успешно отправлено" });
        }*/

        /// <summary>
        /// Получить последние N сообщений в клане.
        /// </summary>
        /// <param name="messagesCount">Количество сообщений</param>
        [HttpGet("lastMessages/{messagesCount}")]
        [ProducesResponseType(typeof(List<MessageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LastMessages(int messagesCount)
        {
            if (messagesCount <= 0) return BadRequest(new { message = "Кол-во сообщений должно быть больше 0" });

            var getMessagesResult = await messageService.GetLastMessagesAsync(messagesCount);
            if (getMessagesResult.IsFailure)
            {
                if (getMessagesResult.Error == "Сообщения не найдены") return NotFound(new { message = getMessagesResult.Error });
                return BadRequest(new { message = getMessagesResult.Error });
            }
            return Ok(getMessagesResult.Value);
        }
    }
}
