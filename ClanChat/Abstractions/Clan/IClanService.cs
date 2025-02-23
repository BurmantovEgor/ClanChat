using ClanChat.Core.DTOs.Clan;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanService
    {
        Task<Result<List<ClanDTO>>> GetAllAsync();
        Task<Result<ClanDTO>> CreateNewAsync(CreateClanDTO dto);
        Task<Result<ClanDTO>> FindByIdAsync(Guid clanId);

    }
}
