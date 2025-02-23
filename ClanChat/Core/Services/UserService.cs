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

        /// <summary>
        /// Валидация данных и смена клана текущего пользователя
        /// </summary>
        public async Task<Result<AuthUserDTO>> ChangeClanAsync(Guid clanId)
        {
            var userClaim = _httpContextAccessor.HttpContext?.User;
            var userGuid = await CheckUserIdClaim(userClaim);
            if (userGuid.IsFailure) return Result.Failure<AuthUserDTO>(userGuid.Error);

            var updResult = await _userRepository.ChangeClanAsync(userGuid.Value, clanId);
            if (!updResult.Succeeded) return Result.Failure<AuthUserDTO>(updResult.Errors.First().Description);

            var newUser = await _userRepository.FindByIdAsync(userGuid.Value);
            if (newUser == null) return Result.Failure<AuthUserDTO>("Пользователь не найден");

            var userDTO = _mapper.Map<AuthUserDTO>(newUser);
            userDTO.UserToken = _tokenService.GenerateJwtToken(newUser);

            return Result.Success(userDTO);
        }


        /// <summary>
        /// Поиск пользоваеля по ID
        /// </summary>
        public async Task<Result<UserDTO>> FindByIdAsync(Guid userId)
        {
            var userEntity = await _userRepository.FindByIdAsync(userId);
            if (userEntity == null) return Result.Failure<UserDTO>("Пользователь не найден");
            var userDto = _mapper.Map<UserDTO>(userEntity);
            return Result.Success(userDto);
        }

        /// <summary>
        /// Валидация данных и проверка данных авторизации пользователя в БД 
        /// </summary>
        public async Task<Result<AuthUserDTO>> LoginAsync(LoginUserDTO user)
        {
            var currentUser = await _userRepository.FindByNameAsync(user.UserName);
            if (currentUser == null) return Result.Failure<AuthUserDTO>("Пользователь не найден");

            var checkPassword = await _userRepository.CheckPasswordAsync(currentUser, user.Password);
            if (!checkPassword)  return Result.Failure<AuthUserDTO>("Неверный пароль");

            var authUser = _mapper.Map<AuthUserDTO>(currentUser);
            authUser.UserToken = _tokenService.GenerateJwtToken(currentUser);

            return Result.Success(authUser);
        }

        /// <summary>
        /// Валидация данных и сохранения пользователя в БД при регистрации
        /// </summary>
        public async Task<Result<AuthUserDTO>> RegisterAsync(RegisterUserDTO user)
        {
            var existingUser = await _userRepository.FindByNameAsync(user.UserName);
            if (existingUser != null) return Result.Failure<AuthUserDTO>("Пользователь с таким именем уже существует");

            var clanCheck = await _clanService.FindByIdAsync(user.ClanId);
            if (clanCheck.IsFailure) return Result.Failure<AuthUserDTO>("Клан не найден");

            var userEntity = _mapper.Map<UserEntity>(user);

            var userCreationResult = await _userRepository.CreateAsync(userEntity, user.Password);
            if (!userCreationResult.Succeeded)
                return Result.Failure<AuthUserDTO>(userCreationResult.Errors.First().Description);

            var authUser = _mapper.Map<AuthUserDTO>(userEntity);
            authUser.UserToken = _tokenService.GenerateJwtToken(userEntity);

            return Result.Success(authUser);
        }

        /// <summary>
        /// Проверка наличия валидного ID пользоваетя в Claims
        /// </summary>
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
