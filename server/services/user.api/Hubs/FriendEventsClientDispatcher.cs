using MediatR;
using Microsoft.AspNetCore.SignalR;
using user.api.Hubs.Interfaces;
using user.bll.Infrastructure.Events;
using user.bll.Infrastructure.ViewModels;

namespace user.api.Hubs
{
    /// <summary>
    /// Manage friend related events
    /// </summary>
    public class FriendEventsClientDispatcher :
        INotificationHandler<AddFriendEvent>,
        INotificationHandler<RemoveFriendEvent>,
        INotificationHandler<RefreshFriendsEvent>,
        INotificationHandler<OfflineEvent>
    {
        private readonly IHubContext<FriendEventsHub, IEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public FriendEventsClientDispatcher(IHubContext<FriendEventsHub, IEventsHub> context)
        {
            _context = context;
        }

        /// <summary>
        /// Handle friend request events
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(AddFriendEvent notification, CancellationToken cancellationToken)
        {
            return _context.Clients.Group(notification.ReceiverId.ToString()).FriendRequest(notification.SenderUser, cancellationToken);
        }

        /// <summary>
        /// Handle friend remove events
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(RemoveFriendEvent notification, CancellationToken cancellationToken)
        {
            return _context.Clients.Group(notification.ReceiverId.ToString()).FriendRemove(new UserNameViewModel { Id = notification.SenderId }, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(RefreshFriendsEvent notification, CancellationToken cancellationToken)
        {
            var friendIds = new List<string>();
            friendIds.AddRange(notification.FriendList.Received.Select(f => f.Id.ToString()));
            friendIds.AddRange(notification.FriendList.Sent.Select(f => f.Id.ToString()));
            friendIds.AddRange(notification.FriendList.Friends.Select(f => f.Id.ToString()));
            await _context.Clients.Groups(friendIds).FriendStatus(new FriendStatusViewModel { Id = notification.UserId, Status = true }, cancellationToken);

            var list = friendIds.Select(x => new FriendStatusViewModel { Id = x, Status = Connections.UserConnections.Exists(x) }).ToList();
            await _context.Clients.Group(notification.UserId).FriendStatuses(list, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(OfflineEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Groups(notification.Friends).FriendStatus(new FriendStatusViewModel { Id = notification.UserId, Status = false }, cancellationToken);
        }
    }
}
