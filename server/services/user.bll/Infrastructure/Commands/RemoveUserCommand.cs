using MediatR;
using System.Security.Claims;

namespace user.bll.Infrastructure.Commands
{
    public class RemoveUserCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public RemoveUserCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
