using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;

namespace ClanChat.Core.Services
{
    public class UserService(IUserRepository userRepository, 
        UserManager<UserEntity> userManager, 
        ITokenService tokenService, 
        IClanService clanService,
        IMapper mapper) : IUserService
    {
        public async Task<Result<AuthResponseDTO>> Login(LoginUserDTO user)
        {
            var currentUser = await userManager.FindByNameAsync(user.UserName);
            if (currentUser == null) return Result.Failure<AuthResponseDTO>("User with this username dont exists.");
            var checkPass = await userManager.CheckPasswordAsync(currentUser, user.Password);
            if(!checkPass) return Result.Failure<AuthResponseDTO>("Incorrect Password");
            var authUser = mapper.Map<AuthResponseDTO>(currentUser);
            authUser.UserToken = tokenService.GenerateJwtToken(currentUser);
            return Result.Success<AuthResponseDTO>(authUser);
        }

        public async Task<Result<AuthResponseDTO>> Register(CreateUserDTO user)
        {
            var existingUser = await userManager.FindByNameAsync(user.UserName);
            if (existingUser != null) return Result.Failure<AuthResponseDTO>("User with this username already exists.");
            var clan = await clanService.FindByIdAsync(user.ClanId);
            if (clan.IsFailure) return Result.Failure<AuthResponseDTO>(clan.Error);
            var user1 = new UserEntity
            {
                UserName = user.UserName,
                ClanId = user.ClanId
            };
            var result = await userManager.CreateAsync(user1, user.Password);
            if (result.Errors.Any()) return Result.Failure<AuthResponseDTO>(result.Errors.First().ToString());
            var authUser = mapper.Map<AuthResponseDTO>(user1);
            authUser.UserToken = tokenService.GenerateJwtToken(user1);
            return Result.Success<AuthResponseDTO>(authUser);
        }
    }
}
