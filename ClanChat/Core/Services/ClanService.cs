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

        public async Task<Result<ClanDTO>> CreateNewAsync(CreateClanDTO clanDTO)
        {
            var checkClan = _clanRepository.FindByNameAsync(clanDTO.Name);
            if (checkClan != null) return Result.Failure<ClanDTO>("Клан с таким именем уже существует");

            var createClanModel = ClanModel.Create(clanDTO);
            if (createClanModel.IsFailure)
                return Result.Failure<ClanDTO>($"Ошибка при создании клана: {createClanModel.Error}");

            var clanEntity = _mapper.Map<ClanEntity>(createClanModel.Value);

            var creationResult = await _clanRepository.CreateNewAsync(clanEntity);
            if (creationResult == 0)
                return Result.Failure<ClanDTO>("Ошибка сохранения в БД");

            var newClan = await FindByIdAsync(clanEntity.Id);
            return newClan.IsFailure ? Result.Failure<ClanDTO>("Не удалось найти новый клан") : newClan;
        }

        public async Task<Result<ClanDTO>> FindByIdAsync(Guid clanId)
        {
            var clanEntity = await _clanRepository.FindByIdAsync(clanId);
            if (clanEntity == null)
                return Result.Failure<ClanDTO>($"Клан с ID {clanId} не найден");

            var clanDTO = _mapper.Map<ClanDTO>(clanEntity);
            return Result.Success(clanDTO);
        }

        public async Task<Result<List<ClanDTO>>> GetAllAsync()
        {
            var clanEntities = await _clanRepository.GetAllAsync();
            if (clanEntities.Count == 0)
                return Result.Failure<List<ClanDTO>>("Кланы не найдены");

            var clanDTOs = _mapper.Map<List<ClanDTO>>(clanEntities);
            return Result.Success(clanDTOs);
        }
    }
}
