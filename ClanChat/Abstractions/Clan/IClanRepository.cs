using ClanChat.Core.Models;
using ClanChat.Core.DTOs;
using CSharpFunctionalExtensions;
using ClanChat.Data.Entities;
using ClanChat.Core.DTOs.Clan;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanRepository
    {
        Task<List<ClanEntity>> GetAll();
        Task<int> CreateNew(ClanEntity clanModell);
        Task<ClanEntity> FindByIdAsync(Guid clanId);

    }
}
