using AutoMapper;
using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;

namespace game.bll.MappingProfiles
{
    public class GameProfile : Profile
    {
        public GameProfile() {

            CreateMap<Game, GameViewModel>()
                .AfterMap<PlayerOrderAction>();
            CreateMap<CreateGameDTO, Game>();
        }

        private class PlayerOrderAction : IMappingAction<Game, GameViewModel>
        {
            public void Process(Game source, GameViewModel destination, ResolutionContext context)
            {
                var actualId = source.PlayerIds.ToList().IndexOf(destination.ActualPlayerId);
                if (actualId != -1)
                {
                    var fistPart = destination.Players.Take(actualId);
                    var secondPart = destination.Players.Skip(actualId);
                    destination.Players = secondPart.Concat(fistPart);
                }
            }
        }
    }
}
