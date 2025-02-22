using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace ClanChat.Abstractions.User
{
    public interface IUserService
    {
        Task<Result<AuthUserDTO>> Register(RegisterUserDTO user);
        Task<Result<AuthUserDTO>> Login(LoginUserDTO user);
        Task<Result<AuthUserDTO>> ChangeClan(Guid userId, Guid clanId);
    }
}
