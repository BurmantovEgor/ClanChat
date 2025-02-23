using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.Message;
using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using ClanChat.Helpers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ClanChat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IClanService _clanService;
        private readonly IUserService _userService;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageService(IMessageRepository messageRepository,
                              IHttpContextAccessor httpContextAccessor,
                              IMapper mapper,
                              IHubContext<MessageHub> messageHub,
                              IClanService clanService,
                              IUserService userService

            )
        {
            _messageRepository = messageRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _messageHub = messageHub;
            _clanService = clanService;
            _userService = userService;
        }



        public async Task<Result<List<MessageDTO>>> GetLastMessagesAsync(int count)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var clanGuid = await CheckClanIdClaim(user);
            if (clanGuid.IsFailure) return Result.Failure<List<MessageDTO>>(clanGuid.Error);

            var messages = await _messageRepository.GetLastMessagesAsync(count, clanGuid.Value);
            if (!messages.Any()) return Result.Failure<List<MessageDTO>>("Сообщения не найдены");
            return Result.Success(messages);
        }

        public async Task<Result> SendMessageAsync(CreateMessageDTO dto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var clanGuid = await CheckClanIdClaim(user);
            if (clanGuid.IsFailure) return Result.Failure<List<MessageDTO>>(clanGuid.Error);

            var userGuid = await CheckUserIdClaim(user);
            if (userGuid.IsFailure) return Result.Failure<List<MessageDTO>>(userGuid.Error);

            var messageEntity = _mapper.Map<MessageEntity>(dto, opts =>
            {
                opts.Items["UserId"] = userGuid.Value;
                opts.Items["ClanId"] = clanGuid.Value;
            });

            var result = await _messageRepository.SaveNewMessageAsync(messageEntity);
            if (result == 0) return Result.Failure("Ошибка сохранения в БД");

            var savedMessage = await _messageRepository.GetByIdAsync(messageEntity.Id);
            if (savedMessage == null) return Result.Failure("Сообщение не найдено");

            await _messageHub.Clients.Group($"clan-{clanGuid.Value}").SendAsync("ReceiveNewMessage", savedMessage);

            return Result.Success();
        }

        private async Task<Result<Guid>> CheckClanIdClaim(ClaimsPrincipal user)
        {
            var clanIdClaim = user?.FindFirst("ClanId")?.Value;
            var clanGuid = new Guid(clanIdClaim);
            var clanCheck = await _clanService.FindByIdAsync(clanGuid);
            if (clanCheck.IsFailure) return Result.Failure<Guid>("Клан не найден");
            return Result.Success(clanGuid);

        }
        private async Task<Result<Guid>> CheckUserIdClaim(ClaimsPrincipal user)
        {
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userGuid = new Guid(userIdClaim);
            var userCheck = await _userService.FindByIdAsync(userGuid);
            if (userCheck.IsFailure) return Result.Failure<Guid>("Пользователь не найден");
            return Result.Success(userGuid);
        }
    }
}
