using MediatR;
using shared.dal.Repository.Interfaces;
using user.bll.Infrastructure.Events;

namespace user.bll.Infrastructure
{
    public class HistoryNotificationHandler :
        INotificationHandler<RefreshHistoryEvent>
    {
        private readonly ICacheRepository _cacheRepository;

        public HistoryNotificationHandler(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task Handle(RefreshHistoryEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Delete($"history-{notification.UserId}", cancellationToken);
        }
    }
}
