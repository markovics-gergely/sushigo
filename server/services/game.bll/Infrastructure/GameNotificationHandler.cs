using game.bll.Extensions;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Events;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using shared.dal.Repository.Interfaces;

namespace game.bll.Infrastructure
{
    public class GameNotificationHandler :
        INotificationHandler<RefreshGameEvent>,
        INotificationHandler<RemoveGameEvent>,
        INotificationHandler<EndRoundEvent>,
        INotificationHandler<EndTurnEvent>
    {
        private readonly ILogger _logger;
        private readonly ICacheRepository _cacheRepository;

        public GameNotificationHandler(ILogger<GameNotificationHandler> logger, ICacheRepository cacheRepository)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
        }

        public async Task Handle(RefreshGameEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Put($"game-{notification.GameViewModel.Id}", notification.GameViewModel, null, cancellationToken);
        }

        public async Task Handle(RemoveGameEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Delete($"game-{notification.GameId}", cancellationToken);
        }

        public Task Handle(EndRoundEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] END ROUND EVENT");
            var command = new ProceedEndRoundCommand
            {
                IsJob = true,
                GameId = notification.GameId
            };
            BackgroundJob.Schedule<MediatorHangfireBridge>(bridge => bridge.Send($"end-round-{notification.GameId}", command), DateTime.Now.AddSeconds(30));
            return Task.CompletedTask;
        }

        public Task Handle(EndTurnEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] END TURN EVENT");
            var command = new ProceedEndTurnCommand
            {
                IsJob = true,
                GameId = notification.GameId
            };
            BackgroundJob.Schedule<MediatorHangfireBridge>(bridge => bridge.Send($"end-turn-{notification.GameId}", command), DateTime.Now.AddSeconds(30));
            return Task.CompletedTask;
        }
    }
}
