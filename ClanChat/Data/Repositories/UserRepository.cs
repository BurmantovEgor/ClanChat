using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.Repositories
{
    public class UserRepository(ClanChatDbContext dbContext, UserManager<UserEntity> userManager) : IUserRepository
    {

        public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<UserEntity> FindByNameAsync(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<bool> CheckPasswordAsync(UserEntity user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

    }
}
