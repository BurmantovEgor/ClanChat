using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.Data.Repositories
{
    public class UserRepository(ClanChatDbContext dbContext) : IUserRepository
    {
        public async Task<UserEntity> GetByUserName(string UserName)
        {
            var result = await dbContext.User.FirstOrDefaultAsync(x => x.UserName == UserName);
            return result;
        }

        public Task<Result<AuthResponseDTO>> Register(CreateUserDTO user)
        {
            throw new NotImplementedException();
        }

  
    }
}
