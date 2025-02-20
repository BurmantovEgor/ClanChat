using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs;
using ClanChat.Core.Models;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.Repositories
{
    public class ClanRepository(ClanChatDbContext dbContext) : IClanRepository
    {
        public async Task<Result> CreateNew(ClanEntity clanModell)
        {
           await dbContext.Clan.AddAsync(clanModell);
            var result = await dbContext.SaveChangesAsync();
            if (result == 0) return Result.Failure("DB Error");
            return Result.Success();
        }

        public async Task<Result<ClanEntity>> FindByIdAsync(Guid clanId)
        {
            var currClan = await dbContext.Clan.FirstOrDefaultAsync(x => x.Id == clanId);
            if (currClan == null) return Result.Failure<ClanEntity>("Not found");
            return Result.Success(currClan);
        }

        public async Task<Result<List<ClanEntity>>> GetAll()
        {
            var clans = await dbContext.Clan.ToListAsync();
            if (clans == null) return Result.Failure<List<ClanEntity>>("DB Error");
            return Result.Success(clans);
        }
    }
}
