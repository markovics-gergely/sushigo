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
            CreateMap<Lobby, LobbyViewModel>();
            CreateMap<LobbyDTO, Lobby>().ReverseMap();
        }
    }
}
