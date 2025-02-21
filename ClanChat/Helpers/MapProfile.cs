using AutoMapper;
using ClanChat.Core.DTOs.Clan;
using ClanChat.Core.DTOs.Message;
using ClanChat.Core.DTOs.User;
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

            CreateMap<UserEntity, AuthResponseDTO>();
            CreateMap<CreateUserDTO, UserEntity>();


            CreateMap<NewMessageDTO, MessageEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom((src, dest, _, context) => context.Items["UserId"]))
                .ForMember(dest => dest.ClanId, opt => opt.MapFrom((src, dest, _, context) => context.Items["ClanId"]));


            CreateMap<UserEntity, UserDTO>();
            CreateMap<MessageEntity, MessageDTO>();


        }
    }
}
