using AutoMapper;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.Domain;

namespace lobby.bll.MappingProfiles
{
    public class LobbyProfile : Profile
    {
        public LobbyProfile() {
            CreateMap<Lobby, LobbyItemViewModel>();
            CreateMap<Lobby, LobbyViewModel>()
                .ForMember(d => d.CreatorUserName, opt => opt.MapFrom(v => v.Players.First(p => p.UserId == v.CreatorUserId).UserName));
            CreateMap<LobbyDTO, Lobby>().ReverseMap();
        }
    }
}
