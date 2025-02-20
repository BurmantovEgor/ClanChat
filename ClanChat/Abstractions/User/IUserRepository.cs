using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.User
{
    public interface IUserRepository
    {
        Task<Result<AuthResponseDTO>> Register(CreateUserDTO user);
        Task<UserEntity> GetByUserName(string UserName);

    }
}
