using AutoMapper;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.Domain;

namespace lobby.bll.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile() {
            CreateMap<Player, PlayerViewModel>();
            CreateMap<PlayerDTO, Player>();
        }
    }
}
