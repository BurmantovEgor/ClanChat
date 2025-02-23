using ClanChat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClanChat.Abstractions.User
{
    public interface IUserRepository
    {
        Task<UserEntity> FindByNameAsync(string username);
        Task<bool> CheckPasswordAsync(UserEntity user, string password);
        Task<IdentityResult> CreateAsync(UserEntity user, string password);
        Task<IdentityResult> ChangeClan(Guid userId, Guid clanId);
        Task<UserEntity> FindByIdAsync(Guid userId);
    }
}
