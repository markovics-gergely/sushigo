using AutoMapper;
using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;

namespace game.bll.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile() {
            CreateMap<Player, PlayerViewModel>();
            CreateMap<CreatePlayerDTO, Player>();
        }
    }
}
