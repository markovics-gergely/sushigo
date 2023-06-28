using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class CreateGameCommand : IRequest<GameViewModel>
    {
        public ClaimsPrincipal? User { get; }
        public CreateGameDTO CreateGameDTO { get; }
        public CreateGameCommand(CreateGameDTO createGameDTO, ClaimsPrincipal? user = null)
        {
            CreateGameDTO = createGameDTO;
            User = user;
        }
    }
}
