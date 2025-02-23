using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs.Clan;
using ClanChat.Core.Models;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Core.Services
{
    public class ClanService : IClanService
    {
        private readonly IMapper _mapper;
        private readonly IClanRepository _clanRepository;

        public ClanService(IMapper mapper, IClanRepository clanRepository)
        {
            _mapper = mapper;
            _clanRepository = clanRepository;
        }

        /// <summary>
        /// Создание нового клана
        /// </summary>
        public async Task<Result<ClanDTO>> CreateNewAsync(CreateClanDTO newClanDTO)
        {
            var checkClanName = await _clanRepository.FindByNameAsync(newClanDTO.Name);
            if (checkClanName != null) return Result.Failure<ClanDTO>("Клан с таким именем уже существует");

            var createdClanModel = ClanModel.Create(newClanDTO);
            if (createdClanModel.IsFailure)
                return Result.Failure<ClanDTO>($"Ошибка при создании клана: {createdClanModel.Error}");

            var clanEntity = _mapper.Map<ClanEntity>(createdClanModel.Value);

            var creationResult = await _clanRepository.CreateNewAsync(clanEntity);
            if (creationResult == 0)
                return Result.Failure<ClanDTO>("Ошибка сохранения в БД");

            var newClan = await FindByIdAsync(clanEntity.Id);
            return newClan.IsFailure ? Result.Failure<ClanDTO>("Не удалось найти новый клан") : newClan;
        }

        /// <summary>
        /// Поиск клана по ID 
        /// </summary>
        public async Task<Result<ClanDTO>> FindByIdAsync(Guid clanId)
        {
            var clanDTO = await _clanRepository.FindByIdAsync(clanId);
            if (clanDTO == null)
                return Result.Failure<ClanDTO>($"Клан с ID {clanId} не найден");

            return Result.Success(clanDTO);
        }

        /// <summary>
        /// Получение списка всех кланов 
        /// </summary>
        public async Task<Result<List<ClanDTO>>> GetAllAsync()
        {
            var clanDTOs = await _clanRepository.GetAllAsync();
            if (clanDTOs.Count == 0)
                return Result.Failure<List<ClanDTO>>("Кланы не найдены");

            return Result.Success(clanDTOs);
        }
    }
}
