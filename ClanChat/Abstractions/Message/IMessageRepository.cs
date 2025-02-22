using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageRepository
    {

        Task<List<MessageDTO>> GetLastMessages(int count, Guid clanId, Guid userId);
        Task<int> SaveNewMessage(MessageEntity msgEntity);
        Task<MessageDTO> GetById(Guid messageId, Guid userId);

    }
}
