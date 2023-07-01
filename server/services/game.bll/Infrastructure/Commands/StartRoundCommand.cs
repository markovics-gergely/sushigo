using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class StartRoundCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public StartRoundCommand(ClaimsPrincipal? user = null) {
            User = user;
        }
    }
}
