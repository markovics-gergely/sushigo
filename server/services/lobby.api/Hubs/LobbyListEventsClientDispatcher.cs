using lobby.api.Hubs.Interfaces;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// Hub dispatcher for lobby list related events
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
        /// Event handler for adding a lobby to the lobby list
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(AddLobbyEvent notification, CancellationToken cancellationToken)
        {
            var lobbyItem = new LobbyItemViewModel
            {
                Id = notification.Lobby.Id,
                Name = notification.Lobby.Name,
            };
            await _context.Clients.All.AddLobby(lobbyItem, cancellationToken);
        }

        /// <summary>
        /// Event handler for removing a lobby from the lobby list
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
