using AutoMapper;
using game.bll.Infrastructure.Events;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using shared.bll.Settings;
using System.Text;

namespace game.bll.Infrastructure
{
    public class GameNotificationHandler :
        INotificationHandler<RefreshGameEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;

        public GameNotificationHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator, IDistributedCache cache, IOptions<CacheSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _settings = settings.Value;
        }

        public async Task Handle(RefreshGameEvent notification, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(notification.GameViewModel));
            await _cache.SetAsync($"game-{notification.GameViewModel.Id}", serializedData, options, cancellationToken);
        }
    }
}
