using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class EndGameCommand : IRequest<UserViewModel>
    {
        public GameEndDTO GameEndDTO { get; }

        public EndGameCommand(GameEndDTO gameEnd)
        {
            GameEndDTO = gameEnd;
        }
    }
}
