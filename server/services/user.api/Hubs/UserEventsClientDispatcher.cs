using MediatR;
using Microsoft.AspNetCore.SignalR;
using user.api.Hubs.Interfaces;
using user.bll.Infrastructure.Events;

namespace user.api.Hubs
{
    /// <summary>
    /// Manage friend related events
    /// </summary>
    public class UserEventsClientDispatcher :
        INotificationHandler<RefreshUserEvent>,
        INotificationHandler<RemoveGameEvent>
    {
        private readonly IHubContext<FriendEventsHub, IEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public UserEventsClientDispatcher(IHubContext<FriendEventsHub, IEventsHub> context)
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
            return _context.Clients.Group(notification.UserId).RefreshUser();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task Handle(RemoveGameEvent notification, CancellationToken cancellationToken)
        {
            return _context.Clients.Group(notification.UserId).RemoveGame();
        }
    }
}
