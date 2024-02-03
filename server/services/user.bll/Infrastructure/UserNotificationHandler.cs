using MediatR;
using shared.dal.Repository.Interfaces;
using user.bll.Infrastructure.Events;

namespace user.bll.Infrastructure
{
    public class UserNotificationHandler :
        INotificationHandler<RefreshUserEvent>,
        INotificationHandler<RemoveUserEvent>
    {
        private readonly ICacheRepository _cacheRepository;

        public UserNotificationHandler(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task Handle(RefreshUserEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Put($"user-{notification.UserId}", notification.User, null, cancellationToken);
        }

        public async Task Handle(RemoveUserEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Delete($"user-{notification.UserId}", cancellationToken);
        }
    }
}
