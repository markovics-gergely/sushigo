using AutoMapper;
using game.bll.Extensions;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Events;
using game.dal.UnitOfWork.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using shared.bll.Settings;
using System.Text;

namespace game.bll.Infrastructure
{
    public class GameNotificationHandler :
        INotificationHandler<RefreshGameEvent>,
        INotificationHandler<RemoveGameEvent>,
        INotificationHandler<EndRoundEvent>,
        INotificationHandler<EndTurnEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;
        private readonly ILogger _logger;
        private readonly IBackgroundJobClient _client;

        public GameNotificationHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator, IDistributedCache cache, IOptions<CacheSettings> settings, ILogger<GameNotificationHandler> logger, IBackgroundJobClient client)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _settings = settings.Value;
            _logger = logger;
            _client = client;
        }

        public async Task Handle(RefreshGameEvent notification, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(notification.GameViewModel));
            await _cache.SetAsync($"game-{notification.GameViewModel.Id}", serializedData, options, cancellationToken);
        }

        public async Task Handle(RemoveGameEvent notification, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync($"game-{notification.GameId}", cancellationToken);
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
