using ClanChat.Core.DTOs.Message;
using CSharpFunctionalExtensions;
using System.Security.Claims;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageService
    {
        Task<Result> SendMessageAsync(ClaimsPrincipal user,CreateMessageDTO dto);
        Task<Result<List<MessageDTO>>> GetLastMessagesAsync(int count);
    }
}
