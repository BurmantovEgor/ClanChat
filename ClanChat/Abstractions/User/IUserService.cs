using ClanChat.Core.DTOs.User;
using CSharpFunctionalExtensions;

namespace ClanChat.Abstractions.User
{
    public interface IUserService
    {
        Task<Result<AuthResponseDTO>> Register(CreateUserDTO user);
        Task<Result<AuthResponseDTO>> Login(LoginUserDTO user);

    }
}
