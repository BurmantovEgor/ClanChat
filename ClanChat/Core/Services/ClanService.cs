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

        public async Task<Result<ClanDTO>> CreateNew(ClanDTO clanDTO)
        {
            var createClanModel = ClanModel.Create(clanDTO);
            if (createClanModel.IsFailure) return Result.Failure<ClanDTO>($"Ошибка создания клана: {createClanModel.Error}");

            var clanEntity = _mapper.Map<ClanEntity>(createClanModel.Value);
            var result = await _clanRepository.CreateNew(clanEntity);
            if (result == 0) return Result.Failure<ClanDTO>("Не удалось создать новый клан. Пожалуйста, попробуйте позже.");

            return Result.Success(clanDTO);
        }

        public async Task<Result<ClanDTO>> FindByIdAsync(Guid clanId)
        {
            var result = await _clanRepository.FindByIdAsync(clanId);
            if (result == null) return Result.Failure<ClanDTO>($"Клан с идентификатором {clanId} не найден.");
           
            var currClan = _mapper.Map<ClanDTO>(result);
            return Result.Success(currClan);
        }

        public async Task<Result<List<ClanEntity>>> GetAll()
        {
            var result = await _clanRepository.GetAll();
            if (result.Count == 0) return Result.Failure<List<ClanEntity>>("Не найдено ни одного клана.");
           
            return Result.Success(result);
        }
    }
}
