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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IClanService _clanService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           ITokenService tokenService,
                           IClanService clanService,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _clanService = clanService;
            _mapper = mapper;
        }

        public async Task<Result<AuthUserDTO>> ChangeClan(Guid userId, Guid clanId)
        {
            var updResult = await _userRepository.ChangeClan(userId, clanId);
            if (!updResult.Succeeded) return Result.Failure<AuthUserDTO>(updResult.Errors.First().Description);

            var newUser = await _userRepository.FindByIdAsync(userId);
            if (newUser == null) return Result.Failure<AuthUserDTO>("Не удалось получить данные пользователя");

            var userDTO = _mapper.Map<AuthUserDTO>(newUser);
            userDTO.UserToken = _tokenService.GenerateJwtToken(newUser);

            return Result.Success(userDTO);
        }

        public async Task<Result<AuthUserDTO>> Login(LoginUserDTO user)
        {
            var currentUser = await _userRepository.FindByNameAsync(user.UserName);
            if (currentUser == null) return Result.Failure<AuthUserDTO>("Пользователь с таким именем не найден");

            var checkPass = await _userRepository.CheckPasswordAsync(currentUser, user.Password);
            if (!checkPass)  return Result.Failure<AuthUserDTO>("Неверный пароль");

            var authUser = _mapper.Map<AuthUserDTO>(currentUser);
            authUser.UserToken = _tokenService.GenerateJwtToken(currentUser);

            return Result.Success(authUser);
        }

        public async Task<Result<AuthUserDTO>> Register(RegisterUserDTO user)
        {
            var existingUser = await _userRepository.FindByNameAsync(user.UserName);
            if (existingUser != null) return Result.Failure<AuthUserDTO>("Пользователь с таким именем уже существует");

            var clan = await _clanService.FindByIdAsync(user.ClanId);
            if (clan.IsFailure) return Result.Failure<AuthUserDTO>(clan.Error);

            var user1 = _mapper.Map<UserEntity>(user);

            // Создаем нового пользователя
            var result = await _userRepository.CreateAsync(user1, user.Password);
            if (!result.Succeeded)
                return Result.Failure<AuthUserDTO>(result.Errors.First().Description);

            // Маппим пользователя в DTO и генерируем токен
            var authUser = _mapper.Map<AuthUserDTO>(user1);
            authUser.UserToken = _tokenService.GenerateJwtToken(user1);

            return Result.Success(authUser);
        }
    }
}
