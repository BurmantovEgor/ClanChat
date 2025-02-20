using AutoMapper;
using ClanChat.Core.DTOs;
using ClanChat.Core.Models;
using ClanChat.Data.Entities;

namespace ClanChat.Helpers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<ClanModel, ClanEntity>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())).ReverseMap();
            CreateMap<ClanEntity, ClanDTO>();
        }
    }
}
