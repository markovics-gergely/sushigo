using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class CalculateEndTurnCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public CalculateEndTurnCommand(ClaimsPrincipal? user = null) {
            User = user;
        }
    }
}
