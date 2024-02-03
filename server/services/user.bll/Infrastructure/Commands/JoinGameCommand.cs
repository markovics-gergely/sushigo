using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class JoinGameCommand : IRequest<UserViewModel>
    {
        public GameJoinedSingleDTO GameJoinedSingleDTO { get; }

        public JoinGameCommand(GameJoinedSingleDTO gameJoinedSingleDTO)
        {
            GameJoinedSingleDTO = gameJoinedSingleDTO;
        }
    }
}
