using ClanChat.Abstractions.Clan;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.Repositories
{
    public class ClanRepository : IClanRepository
    {
        ClanChatDbContext _dbContext;

        public ClanRepository(ClanChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateNew(ClanEntity clanModell)
        {
            await _dbContext.Clan.AddAsync(clanModell);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<ClanEntity> FindByIdAsync(Guid clanId)
        {
            var currClan = await _dbContext.Clan
                .FirstOrDefaultAsync(x => x.Id == clanId);
            return currClan;
        }

        public async Task<List<ClanEntity>> GetAll()
        {
            var clans = await _dbContext.Clan.ToListAsync();
            return clans;
        }
    }
}
