using lobby.api.Hubs.Interfaces;
using lobby.bll.Infrastructure.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class LobbyEventsClientDispatcher :
        INotificationHandler<AddPlayerEvent>,
        INotificationHandler<RemovePlayerEvent>,
        INotificationHandler<PlayerReadyEvent>,
        INotificationHandler<RemoveLobbyEvent>,
        INotificationHandler<AddMessageEvent>,
        INotificationHandler<UpdateDeckTypeEvent>
    {
        private readonly IHubContext<LobbyEventsHub, ILobbyEventsHub> _context;

        /// <summary>
        /// Add context to dispatcher
        /// </summary>
        /// <param name="context"></param>
        public LobbyEventsClientDispatcher(IHubContext<LobbyEventsHub, ILobbyEventsHub> context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(AddPlayerEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyId.ToString()).AddPlayer(notification.Player, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(RemovePlayerEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyId.ToString()).RemovePlayer(notification.PlayerId, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(PlayerReadyEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyViewModel.Id.ToString()).PlayerReady(notification.LobbyViewModel, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(RemoveLobbyEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyId.ToString()).Removelobby(cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(AddMessageEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyId.ToString()).AddMessage(notification.Message, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UpdateDeckTypeEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.LobbyViewModel.Id.ToString()).UpdateDeckType(notification.LobbyViewModel, cancellationToken);
        }
    }
}
