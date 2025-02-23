using ClanChat.Abstractions.User;
using ClanChat.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClanChat.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        public UserRepository(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<UserEntity> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<UserEntity> FindByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangeClan(Guid userId, Guid clanId)
        {
            var result = await _userManager.FindByIdAsync(userId.ToString());
            result.ClanId = clanId;
            return await _userManager.UpdateAsync(result);
        }

    }
}
