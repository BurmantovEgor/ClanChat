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
    public class MessageRepository(ClanChatDbContext dbContext, IMapper mapper) : IMessageRepository
    {
        public async Task<Result<MessageDTO>> GetById(Guid messageId)
        {
            var result = await dbContext.Message
                .Include(m => m.User)
                .Include(c => c.User.Clan)
                .Where(m => m.Id == messageId)
                .ProjectTo<MessageDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
                
            return result;
        }

        public async Task<List<MessageEntity>> GetLastMessages(int count, Guid clanId)
        {
            
            var result = await dbContext.Message
                .Include(m => m.User)
                .Include(c => c.User.Clan)
                .Where(m => m.ClanId == clanId)
                .OrderByDescending(m => m.CreatedTime)
                .Take(count)
                .ToListAsync();

            return result;
        }

        public async Task<Result> SaveNewMessage(MessageEntity msgEntity)
        {
            await dbContext.Message.AddAsync(msgEntity);
            var result = await dbContext.SaveChangesAsync();
            if (result == 0) return Result.Failure("Не удалось сохранить в бд");
            return Result.Success();
        }
    }
}
