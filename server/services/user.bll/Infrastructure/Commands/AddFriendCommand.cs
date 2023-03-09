using MediatR;
using System.Security.Claims;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class AddFriendCommand : IRequest<UserNameViewModel>
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
