using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClanChat.Abstractions.Message;
using ClanChat.Core.DTOs.Message;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<MessageDTO> GetById(Guid messageId, Guid userId)
        {
            var result = await _dbContext.Message
                .Include(m => m.User)
                .Include(c => c.User.Clan)
                .Where(m => m.Id == messageId)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider, new { CurrentUser = userId })
                .FirstOrDefaultAsync();
            
            result.IsOutgoing = result.User.Id == userId;

            return result;
        }

        public async Task<List<MessageDTO>> GetLastMessages(int count, Guid clanId, Guid userId)
        {
            var messages = await _dbContext.Message
                .Include(m => m.User)
                .Include(c => c.User.Clan)
                .Where(m => m.ClanId == clanId)
                .OrderByDescending(m => m.CreatedTime)
                .Take(count)
                .OrderBy(m => m.CreatedTime)
                .ToListAsync();

            var result = messages.Select(m =>
            {
                var dto = _mapper.Map<MessageDTO>(m);
                dto.IsOutgoing = m.UserId == userId;
                return dto;
            })
                .ToList();

            return result;
        }

        public async Task<int> SaveNewMessage(MessageEntity msgEntity)
        {
            await _dbContext.Message.AddAsync(msgEntity);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}
