using AutoMapper;
using ClanChat.Abstractions.Message;
using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using ClanChat.Helpers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ClanChat.Core.Services
{
    public class MessageService(IMessageRepository messageRepositor, IHttpContextAccessor _httpContextAccessor
        ,IMapper mapper,
        IHubContext<MessageHub> messageHub) : IMessageService
    {
        public async Task<Result<List<MessageDTO>>> GetLastMessages(int count)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var result = user?.FindFirst("ClanId")?.Value;
            var clanGuid = new Guid(result);
            var result213 = await messageRepositor.GetLastMessages(count, clanGuid);
            if (result213.Count() == 0) return Result.Failure<List<MessageDTO>>("Empty list");

            var msgDtoList = mapper.Map<List<MessageDTO>>(result213);

            return Result.Success(msgDtoList);
        }

        public async Task<Result> SendMessage(NewMessageDTO dto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var clanId = user?.FindFirst("ClanId")?.Value;
            var clanGuid = new Guid(clanId);

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userGuid = new Guid(userId);

            var messageEntity = mapper.Map<MessageEntity>(dto, opts =>
            {
                opts.Items["UserId"] = userId;
                opts.Items["ClanId"] = clanId;
            });
            var result = await messageRepositor.SaveNewMessage(messageEntity);
            if (result.IsFailure) return Result.Failure("Не удалось сохранить сообщение");

            var currMessage  = await messageRepositor.GetById(messageEntity.Id);

            await messageHub.Clients.Group($"clan-{clanId}").SendAsync("ReceiveNewMessage", currMessage.Value);

            return result;
        }
    }
}
