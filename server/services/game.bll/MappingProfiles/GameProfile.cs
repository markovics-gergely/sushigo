using AutoMapper;
using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;
using game.dal.Types;

namespace game.bll.MappingProfiles
{
    public class GameProfile : Profile
    {
        private readonly Phase[] phasesWithSelected = { Phase.AfterTurn, Phase.EndTurn };
        public GameProfile() {

            CreateMap<Game, GameViewModel>()
                .AfterMap((src, dest) =>
                {
                    if (!phasesWithSelected.Contains(dest.Phase))
                    {
                        foreach (var player in dest.Players)
                        {
                            player.SelectedCardType = null;
                            player.SelectedCardId = null;
                        }
                    }
                })
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

        private class PlayerAction : IMappingAction<Game, GameViewModel>
        {
            public void Process(Game source, GameViewModel destination, ResolutionContext context)
            {
                
            }
        }
    }
}
