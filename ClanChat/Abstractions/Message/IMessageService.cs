using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageService
    {
        Task<Result> SendMessage(NewMessageDTO dto);
        Task<Result<List<MessageDTO>>> GetLastMessages(int count);
    }
}
