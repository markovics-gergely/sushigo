using MediatR;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Events
{
    public class RefreshFriendsEvent : INotification
    {
        public required string UserId { get; set; }
        public required FriendListViewModel FriendList { get; set; }
    }
}
