using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class ProceedEndTurnCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public ProceedEndTurnCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
