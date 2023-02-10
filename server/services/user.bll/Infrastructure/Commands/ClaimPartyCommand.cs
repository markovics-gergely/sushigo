using MediatR;
using System.Security.Claims;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimPartyCommand : IRequest<bool>
    {
        public ClaimsPrincipal? User { get; set; }

        public ClaimPartyCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
