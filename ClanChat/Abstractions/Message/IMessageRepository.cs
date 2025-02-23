using ClanChat.Core.DTOs.Message;
using ClanChat.Data.Entities;

namespace ClanChat.Abstractions.Message
{
    public interface IMessageRepository
    {

        Task<List<MessageDTO>> GetLastMessagesAsync(int count, Guid clanId);
        Task<int> SaveNewMessageAsync(MessageEntity msgEntity);
        Task<MessageDTO> GetByIdAsync(Guid messageId);

    }
}
