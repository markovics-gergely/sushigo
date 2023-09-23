using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using shared.bll.Settings;
using user.bll.Infrastructure.Events;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Infrastructure
{
    public class HistoryNotificationHandler :
        INotificationHandler<RefreshHistoryEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;

        public HistoryNotificationHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator, IDistributedCache cache, IOptions<CacheSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
            _cache = cache;
            _settings = settings.Value;
        }

        public async Task Handle(RefreshHistoryEvent notification, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync($"history-{notification.UserId}", cancellationToken);
        }
    }
}
