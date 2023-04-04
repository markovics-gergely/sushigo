using MediatR;
using Microsoft.AspNetCore.SignalR;
using user.bll.Infrastructure.Events;

namespace user.api.Hubs
{
    /// <summary>
    /// Manage friend related events
    /// </summary>
    public class UserEventsClientDispatcher :
        INotificationHandler<RefreshUserEvent>
    {
        private readonly IHubContext<FriendEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public UserEventsClientDispatcher(IHubContext<FriendEventsHub> context)
        {
            _context = context;
        }

        /// <summary>
        /// Handle user refresh request events
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Handle(RefreshUserEvent notification, CancellationToken cancellationToken)
        {
            return _context.Clients.Group(notification.UserId).SendAsync("RefreshUser", cancellationToken);
        }
    }
}
