using game.api.Hubs.Interfaces;
using game.bll.Infrastructure.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace game.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class HandEventsClientDispatcher :
        INotificationHandler<RefreshHandEvent>
    {
        private readonly IHubContext<HandEventsHub, IHandEventsHub> _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public HandEventsClientDispatcher(IHubContext<HandEventsHub, IHandEventsHub> context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(RefreshHandEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.PlayerId.ToString()).RefreshHand(notification.Hand, cancellationToken);
        }
    }
}
