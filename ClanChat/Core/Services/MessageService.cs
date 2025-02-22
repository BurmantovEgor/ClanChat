using AutoMapper;
using ClanChat.Abstractions.Message;
using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using ClanChat.Helpers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ClanChat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageService(IMessageRepository messageRepository,
                              IHttpContextAccessor httpContextAccessor,
                              IMapper mapper,
                              IHubContext<MessageHub> messageHub)
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _messageHub = messageHub;
        }

        public async Task<Result<List<MessageDTO>>> GetLastMessages(int count)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var clanIdClaim = user?.FindFirst("ClanId")?.Value;
            var currUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clanIdClaim)) return Result.Failure<List<MessageDTO>>("Не удалось получить информацию о клане");
            var clanGuid = new Guid(clanIdClaim);
            var messages = await _messageRepository.GetLastMessages(count, clanGuid, new Guid(currUserId));
            if (!messages.Any()) return Result.Failure<List<MessageDTO>>("Нет сообщений для отображения");
            return Result.Success(messages);
        }

        public async Task<Result> SendMessage(CreateMessageDTO dto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var clanIdClaim = user?.FindFirst("ClanId")?.Value;
            if (string.IsNullOrEmpty(clanIdClaim)) return Result.Failure("Не удалось получить информацию о клане");

            var clanGuid = new Guid(clanIdClaim);
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) return Result.Failure("Не удалось получить информацию о пользователе.");
            var userGuid = new Guid(userIdClaim);

            var messageEntity = _mapper.Map<MessageEntity>(dto, opts =>
            {
                opts.Items["UserId"] = userIdClaim;
                opts.Items["ClanId"] = clanIdClaim;
            });
            var result = await _messageRepository.SaveNewMessage(messageEntity);

            if (result == 0) return Result.Failure("Не удалось сохранить сообщение. Пожалуйста, попробуйте позже");
            var savedMessage = await _messageRepository.GetById(messageEntity.Id, new Guid(userIdClaim));
            if (savedMessage == null) return Result.Failure("Не удалось получить сохраненное сообщение.");
            await _messageHub.Clients.Group($"clan-{clanIdClaim}").SendAsync("ReceiveNewMessage", savedMessage);

            return Result.Success();
        }
    }
}
