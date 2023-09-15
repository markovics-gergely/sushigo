using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class ProceedEndGameCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public ProceedEndGameCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
