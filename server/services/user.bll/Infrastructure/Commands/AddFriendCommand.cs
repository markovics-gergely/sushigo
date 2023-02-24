using MediatR;
using System.Security.Claims;

namespace user.bll.Infrastructure.Commands
{
    public class AddFriendCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public string FriendName { get; }
        public AddFriendCommand(string friendName, ClaimsPrincipal? user = null)
        {
            FriendName = friendName;
            User = user;
        }
    }
}
