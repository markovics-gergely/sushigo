using AutoMapper;
using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;

namespace game.bll.MappingProfiles
{
    public class GameProfile : Profile
    {
        public GameProfile() {
            CreateMap<Game, GameViewModel>();
            CreateMap<CreateGameDTO, Game>();
        }
    }
}
