using ClanChat.Core.Models;
using ClanChat.Core.DTOs;
using CSharpFunctionalExtensions;
using ClanChat.Data.Entities;
using ClanChat.Core.DTOs.Clan;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanRepository
    {
        Task<Result<List<ClanEntity>>> GetAll();
        Task<Result> CreateNew(ClanEntity clanModell);
        Task<Result<ClanEntity>> FindByIdAsync(Guid clanId);

    }
}
