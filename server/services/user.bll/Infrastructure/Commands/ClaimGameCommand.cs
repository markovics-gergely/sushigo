using MediatR;
using System.Security.Claims;
using user.dal.Types;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimGameCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public GameTypes GameType { get; }

        public ClaimGameCommand(GameTypes gameType, ClaimsPrincipal? user = null)
        {
            GameType = gameType;
            User = user;
        }
    }
}
