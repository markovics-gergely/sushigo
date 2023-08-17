using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class RemoveGameCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public RemoveGameCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
