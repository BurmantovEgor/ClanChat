using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using ClanChat.Helpers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

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
            var currentUser = await userRepository.FindByNameAsync(user.UserName);
            if (currentUser == null)
                return Result.Failure<AuthResponseDTO>("User with this username doesn't exist.");

            var checkPass = await userRepository.CheckPasswordAsync(currentUser, user.Password);
            if (!checkPass)
                return Result.Failure<AuthResponseDTO>("Incorrect Password");

            var authUser = mapper.Map<AuthResponseDTO>(currentUser);
            authUser.UserToken = tokenService.GenerateJwtToken(currentUser);

            return Result.Success<AuthResponseDTO>(authUser);
        }

        public async Task<Result<AuthResponseDTO>> Register(CreateUserDTO user)
        {
            var existingUser = await userRepository.FindByNameAsync(user.UserName);
            if (existingUser != null)
                return Result.Failure<AuthResponseDTO>("User with this username already exists.");

            var clan = await clanService.FindByIdAsync(user.ClanId);
            if (clan.IsFailure)
                return Result.Failure<AuthResponseDTO>(clan.Error);

            var user1 = mapper.Map<UserEntity>(user);
            
            var result = await userRepository.CreateAsync(user1, user.Password);
            if (!result.Succeeded)
                return Result.Failure<AuthResponseDTO>(result.Errors.First().Description);

            var authUser = mapper.Map<AuthResponseDTO>(user1);
            authUser.UserToken = tokenService.GenerateJwtToken(user1);

            return Result.Success<AuthResponseDTO>(authUser);
        }
    }
}
