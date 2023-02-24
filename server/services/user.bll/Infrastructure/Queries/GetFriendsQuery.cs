using MediatR;
using System.Security.Claims;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetFriendsQuery : IRequest<FriendListViewModel>
    {
        public ClaimsPrincipal? User { get; set; }

        public GetFriendsQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
