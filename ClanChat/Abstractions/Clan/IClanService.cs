using ClanChat.Core.DTOs;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanService
    {
        Task<Result<List<ClanEntity>>> GetAll();
        Task<Result<ClanDTO>> CreateNew(ClanDTO dto);
    }
}
