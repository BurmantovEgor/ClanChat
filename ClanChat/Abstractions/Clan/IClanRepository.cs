using ClanChat.Core.Models;
using ClanChat.Core.DTOs;
using CSharpFunctionalExtensions;
using ClanChat.Data.Entities;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanRepository
    {
        Task<Result<List<ClanEntity>>> GetAll();
        Task<Result> CreateNew(ClanEntity clanModell);
    }
}
