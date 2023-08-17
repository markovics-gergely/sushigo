using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class ProceedEndRoundCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public ProceedEndRoundCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
