using AutoMapper;
using ClanChat.Abstractions.Clan;
using ClanChat.Core.DTOs.Clan;
using ClanChat.Core.Models;
using ClanChat.Data.Entities;
using CSharpFunctionalExtensions;

namespace ClanChat.Core.Services
{
    public class ClanService(IMapper mapper, IClanRepository clanRepository) : IClanService
    {
        public async Task<Result<ClanDTO>> CreateNew(ClanDTO clanDTO)
        {
            var createClanModel = ClanModel.Create(clanDTO);
            if (createClanModel.IsFailure) return Result.Failure<ClanDTO>(createClanModel.Error);
            var clanEntity = mapper.Map<ClanEntity>(createClanModel.Value);
            var result = await clanRepository.CreateNew(clanEntity);
            if (result.IsFailure) return Result.Failure<ClanDTO>(result.Error);
            return Result.Success(clanDTO);
        }

        public async Task<Result<ClanDTO>> FindByIdAsync(Guid clanId)
        {
            var result = await clanRepository.FindByIdAsync(clanId);
            if (result.IsFailure) return Result.Failure<ClanDTO>(result.Error);
            var currClan = mapper.Map<ClanDTO>(result.Value);
            return Result.Success(currClan);

        }

        public async Task<Result<List<ClanEntity>>> GetAll()
        {
            var result = await clanRepository.GetAll();
            if (result.IsFailure) return result;
            if (result.Value.Count == 0) return Result.Failure<List<ClanEntity>>("No clans found");
            return Result.Success(result.Value);
        }
    }
}
