using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClanChat.Abstractions.Message;
using ClanChat.Core.DTOs.Message;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ClanChatDbContext _dbContext;
        private readonly IMapper _mapper;
        public MessageRepository(ClanChatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<MessageDTO> GetByIdAsync(Guid messageId)
        {
            var result = await _dbContext.Message
                .Include(m => m.Sender)
                .Include(c => c.Sender.Clan)
                .Where(m => m.Id == messageId)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<MessageDTO>> GetLastMessagesAsync(int count, Guid clanId)
        {
            var messages = await _dbContext.Message
                .Include(m => m.Sender)
                .Include(c => c.Sender.Clan)
                .Where(m => m.ClanId == clanId)
                .OrderByDescending(m => m.CreatedTime)
                .Take(count)
                .OrderBy(m => m.CreatedTime)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return messages;
        }

        public async Task<int> SaveNewMessageAsync(MessageEntity msgEntity)
        {
            await _dbContext.Message.AddAsync(msgEntity);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}
