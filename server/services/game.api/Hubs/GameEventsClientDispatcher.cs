using game.api.Hubs.Interfaces;
using game.bll.Infrastructure.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace game.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class GameEventsClientDispatcher : 
        INotificationHandler<RefreshGameEvent>,
        INotificationHandler<EndTurnEvent>,
        INotificationHandler<EndRoundEvent>
    {
        private readonly IHubContext<GameEventsHub, IGameEventsHub> _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GameEventsClientDispatcher(IHubContext<GameEventsHub, IGameEventsHub> context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(RefreshGameEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.GameViewModel.Id.ToString()).RefreshGame(notification.GameViewModel, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(EndRoundEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.GameId.ToString()).EndRound(cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(EndTurnEvent notification, CancellationToken cancellationToken)
        {
            await _context.Clients.Group(notification.GameId.ToString()).EndTurn(cancellationToken);
        }
    }
}
