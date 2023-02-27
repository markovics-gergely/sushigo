using MediatR;
using Microsoft.AspNetCore.SignalR;
using user.api.Hubs.Interfaces;
using user.bll.Infrastructure.Events;

namespace user.api.Hubs
{
    /// <summary>
    /// Manage friend related events
    /// </summary>
    public class FriendEventsClientDispatcher :
        INotificationHandler<AddFriendEvent>,
        INotificationHandler<RemoveFriendEvent>
    {
        private readonly IHubContext<FriendEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public FriendEventsClientDispatcher(IHubContext<FriendEventsHub> context)
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
            return _context.Clients.Group(notification.ReceiverId.ToString()).SendAsync("FriendRequest", notification.SenderUser, cancellationToken);
        }

        /// <summary>
        /// Handle friend remove events
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(RemoveFriendEvent notification, CancellationToken cancellationToken)
        {
            return _context.Clients.Group(notification.ReceiverId.ToString()).SendAsync("FriendRemove", new { Sender = notification.SenderId }, cancellationToken);
        }
    }
}
