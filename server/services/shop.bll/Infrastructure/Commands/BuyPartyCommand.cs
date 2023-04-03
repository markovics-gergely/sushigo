using MediatR;
using System.Security.Claims;

namespace shop.bll.Infrastructure.Commands
{
    public class BuyPartyCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public BuyPartyCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
