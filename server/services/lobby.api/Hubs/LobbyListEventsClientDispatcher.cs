using lobby.api.Hubs.Interfaces;
using lobby.bll.Infrastructure.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class LobbyListEventsClientDispatcher :
        INotificationHandler<AddLobbyEvent>,
        INotificationHandler<RemoveLobbyEvent>
    {
        private readonly IHubContext<LobbyListEventsHub, ILobbyListEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public LobbyListEventsClientDispatcher(IHubContext<LobbyListEventsHub, ILobbyListEventsHub> context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(AddLobbyEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.All.AddLobby(notification.Lobby, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(RemoveLobbyEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.All.RemoveLobby(notification.LobbyId, cancellationToken);
        }
    }
}
