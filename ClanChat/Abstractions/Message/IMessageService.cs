using ClanChat.Core.DTOs.Message;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageService
    {
        Task<Result> SendMessageAsync(CreateMessageDTO dto);
        Task<Result<List<MessageDTO>>> GetLastMessagesAsync(int count);
    }
}
