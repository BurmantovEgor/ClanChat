using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.User;
using ClanChat.Core.DTOs.Message;
using ClanChat.Core.DTOs.User;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClanChat.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;
        private readonly IClanService _clanService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           ITokenService tokenService,
                           IHttpContextAccessor httpContextAccessor,
                           IClanService clanService,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _clanService = clanService;
            _mapper = mapper;
        }

        public async Task<Result<AuthUserDTO>> ChangeClanAsync(Guid clanId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userGuid = await CheckUserIdClaim(user);
            if (userGuid.IsFailure) return Result.Failure<AuthUserDTO>(userGuid.Error);

            var updResult = await _userRepository.ChangeClan(userGuid.Value, clanId);
            if (!updResult.Succeeded) return Result.Failure<AuthUserDTO>(updResult.Errors.First().Description);

            var newUser = await _userRepository.FindByIdAsync(userGuid.Value);
            if (newUser == null) return Result.Failure<AuthUserDTO>("Пользователь не найден");

            var userDTO = _mapper.Map<AuthUserDTO>(newUser);
            userDTO.UserToken = _tokenService.GenerateJwtToken(newUser);

            return Result.Success(userDTO);
        }



        public async Task<Result<UserDTO>> FindByIdAsync(Guid userId)
        {
            var result = await _userRepository.FindByIdAsync(userId);
            if (result == null) return Result.Failure<UserDTO>("Пользователь не найден");
            var userDto = _mapper.Map<UserDTO>(result);
            return Result.Success(userDto);
        }


        public async Task<Result<AuthUserDTO>> LoginAsync(LoginUserDTO user)
        {
            var currentUser = await _userRepository.FindByNameAsync(user.UserName);
            if (currentUser == null) return Result.Failure<AuthUserDTO>("Пользователь не найден");

            var checkPass = await _userRepository.CheckPasswordAsync(currentUser, user.Password);
            if (!checkPass)  return Result.Failure<AuthUserDTO>("Неверный пароль");

            var authUser = _mapper.Map<AuthUserDTO>(currentUser);
            authUser.UserToken = _tokenService.GenerateJwtToken(currentUser);

            return Result.Success(authUser);
        }

        public async Task<Result<AuthUserDTO>> RegisterAsync(RegisterUserDTO user)
        {
            var existingUser = await _userRepository.FindByNameAsync(user.UserName);
            if (existingUser != null) return Result.Failure<AuthUserDTO>("Пользователь с таким именем уже существует");

            var clan = await _clanService.FindByIdAsync(user.ClanId);
            if (clan.IsFailure) return Result.Failure<AuthUserDTO>("Клан не найден");

            var user1 = _mapper.Map<UserEntity>(user);

            var result = await _userRepository.CreateAsync(user1, user.Password);
            if (!result.Succeeded)
                return Result.Failure<AuthUserDTO>(result.Errors.First().Description);

            var authUser = _mapper.Map<AuthUserDTO>(user1);
            authUser.UserToken = _tokenService.GenerateJwtToken(user1);

            return Result.Success(authUser);
        }

        private async Task<Result<Guid>> CheckUserIdClaim(ClaimsPrincipal user)
        {
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userGuid = new Guid(userIdClaim);
            var userCheck = await _userRepository.FindByIdAsync(userGuid);
            if (userCheck == null) return Result.Failure<Guid>("Пользователь не найден");
            return Result.Success(userGuid);
        }
    }
}
