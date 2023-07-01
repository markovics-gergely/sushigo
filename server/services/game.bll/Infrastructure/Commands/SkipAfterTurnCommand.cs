using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class SkipAfterTurnCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public SkipAfterTurnCommand(ClaimsPrincipal? user = null) {
            User = user;
        }
    }
}
