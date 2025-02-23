using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.User
{
    public interface IUserService
    {
        Task<Result<AuthUserDTO>> RegisterAsync(RegisterUserDTO user);
        Task<Result<AuthUserDTO>> LoginAsync(LoginUserDTO user);
        Task<Result<AuthUserDTO>> ChangeClanAsync(Guid clanId);
        Task<Result<UserDTO>> FindByIdAsync(Guid userId);

    }
}
