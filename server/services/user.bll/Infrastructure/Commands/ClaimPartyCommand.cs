using MediatR;
using System.Security.Claims;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimPartyCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }

        public ClaimPartyCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
