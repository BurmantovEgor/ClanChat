using ClanChat.Core.DTOs.Clan;
using ClanChat.Data.Entities;

namespace ClanChat.Abstractions.Clan
{
    public interface IClanRepository
    {
        Task<List<ClanDTO>> GetAllAsync();
        Task<int> CreateNewAsync(ClanEntity clanModell);
        Task<ClanDTO> FindByIdAsync(Guid clanId);
        Task<ClanDTO> FindByNameAsync(string clanName);

    }
}
