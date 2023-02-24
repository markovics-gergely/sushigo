using MediatR;
using System.Security.Claims;

namespace user.bll.Infrastructure.Commands
{
    public class RemoveFriendCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public Guid FriendId { get; }
        public RemoveFriendCommand(Guid friendId, ClaimsPrincipal? user = null)
        {
            FriendId = friendId;
            User = user;
        }
    }
}
