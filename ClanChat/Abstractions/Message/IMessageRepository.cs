using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageRepository
    {

        Task<List<MessageEntity>> GetLastMessages(int count, Guid clanId);
        Task<Result> SaveNewMessage(MessageEntity msgEntity);
        Task<Result<MessageDTO>> GetById(Guid messageId);

    }
}
