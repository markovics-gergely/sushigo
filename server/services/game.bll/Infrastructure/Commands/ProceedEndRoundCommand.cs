using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class ProceedEndRoundCommand : IRequest
    {
        public bool IsJob { get; set; } = false;
        public Guid? GameId { get; set; }
        public ClaimsPrincipal? User { get; set; }
        public ProceedEndRoundCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
