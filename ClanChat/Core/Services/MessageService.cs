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


        /// <summary>
        /// Получение списка последних N сообщений
        /// </summary>
        public async Task<Result<List<MessageDTO>>> GetLastMessagesAsync(int messagesCount)
        {
            var userClaim = _httpContextAccessor.HttpContext?.User;
            var clanGuid = await CheckClanIdClaim(userClaim);
            if (clanGuid.IsFailure) return Result.Failure<List<MessageDTO>>(clanGuid.Error);

            var messageDTOs = await _messageRepository.GetLastMessagesAsync(messagesCount, clanGuid.Value);
            if (!messageDTOs.Any()) return Result.Failure<List<MessageDTO>>("Сообщения не найдены");
            return Result.Success(messageDTOs);
        }

        /// <summary>
        /// Сохранение сообщения в БД и отправка в чат
        /// </summary>
        public async Task<Result> SendMessageAsync(CreateMessageDTO newMessageDTO)
        {
            var userClaim = _httpContextAccessor.HttpContext?.User;

            var clanGuid = await CheckClanIdClaim(userClaim);
            if (clanGuid.IsFailure) return Result.Failure<List<MessageDTO>>(clanGuid.Error);

            var userGuid = await CheckUserIdClaim(userClaim);
            if (userGuid.IsFailure) return Result.Failure<List<MessageDTO>>(userGuid.Error);

            var messageEntity = _mapper.Map<MessageEntity>(newMessageDTO, opts =>
            {
                opts.Items["UserId"] = userGuid.Value;
                opts.Items["ClanId"] = clanGuid.Value;
            });

            var saveMessageResult = await _messageRepository.SaveNewMessageAsync(messageEntity);
            if (saveMessageResult == 0) return Result.Failure("Ошибка сохранения в БД");

            var messageDTO = await _messageRepository.GetByIdAsync(messageEntity.Id);
            if (messageDTO == null) return Result.Failure("Сообщение не найдено");

            await _messageHub.Clients.Group($"clan-{clanGuid.Value}").SendAsync("ReceiveNewMessage", messageDTO);

            return Result.Success();
        }
        /// <summary>
        /// Проверка наличия валидного ID кана в Claims
        /// </summary>
        private async Task<Result<Guid>> CheckClanIdClaim(ClaimsPrincipal userClaim)
        {
            var clanIdClaim = userClaim?.FindFirst("ClanId")?.Value;
            var clanGuid = new Guid(clanIdClaim);
            var clanCheck = await _clanService.FindByIdAsync(clanGuid);
            if (clanCheck.IsFailure) return Result.Failure<Guid>("Клан не найден");
            return Result.Success(clanGuid);

        }
        /// <summary>
        /// Проверка наличия валидного ID пользоваетя в Claims 
        /// </summary>
        private async Task<Result<Guid>> CheckUserIdClaim(ClaimsPrincipal userClaim)
        {
            var userIdClaim = userClaim?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userGuid = new Guid(userIdClaim);
            var userCheck = await _userService.FindByIdAsync(userGuid);
            if (userCheck.IsFailure) return Result.Failure<Guid>("Пользователь не найден");
            return Result.Success(userGuid);
        }
    }
}
