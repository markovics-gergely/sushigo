using AutoMapper;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using shared.dal.Repository.Interfaces;
using shared.dal.Settings;

namespace lobby.bll.Infrastructure
{
    public class MessageEventHandler :
        INotificationHandler<AddMessageEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;
        private readonly CacheSettings _settings;

        public MessageEventHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheRepository cacheRepository, IOptions<CacheSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
            _settings = settings.Value;
        }

        public async Task Handle(AddMessageEvent notification, CancellationToken cancellationToken)
        {
            // Get messages from database
            var messages = _unitOfWork.MessageRepository.Get(
                    transform: x => x.AsNoTracking().OrderByDescending(m => m.DateTime).Take(_settings.MessageLimit)
                ).ToList();
            var messagesViewModel = _mapper.Map<IEnumerable<MessageViewModel>>(messages);

            // Store messages in cache
            await _cacheRepository.Put($"messages-{notification.LobbyId}", messagesViewModel, null, cancellationToken);
        }
    }
}
