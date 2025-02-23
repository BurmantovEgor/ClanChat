using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs.Clan;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClanChat.Data.Repositories
{
    public class ClanRepository : IClanRepository
    {
        ClanChatDbContext _dbContext;
        IMapper _mapper;
        public ClanRepository(ClanChatDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreateNewAsync(ClanEntity clanModel)
        {
            await _dbContext.Clan.AddAsync(clanModel);
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<ClanDTO> FindByIdAsync(Guid clanId)
        {
            var currClan = await _dbContext.Clan.ProjectTo<ClanDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == clanId);
            return currClan;
        }

        public async Task<ClanDTO> FindByNameAsync(string clanName)
        {
            var currClan = await _dbContext.Clan.ProjectTo<ClanDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Name == clanName);
            return currClan;
        }

        public async Task<List<ClanDTO>> GetAllAsync()
        {
            var clans = await _dbContext.Clan.ProjectTo<ClanDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return clans;
        }
    }
}
