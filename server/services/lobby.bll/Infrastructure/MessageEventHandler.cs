using AutoMapper;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.bll.Settings;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace lobby.bll.Infrastructure
{
    public class MessageEventHandler :
        INotificationHandler<AddMessageEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;

        public MessageEventHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache cache, IOptions<CacheSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _settings = settings.Value;
            _mapper = mapper;
        }

        public async Task Handle(AddMessageEvent notification, CancellationToken cancellationToken)
        {
            var messages = _unitOfWork.MessageRepository.Get(
                    transform: x => x.AsNoTracking().OrderByDescending(m => m.DateTime).Take(_settings.MessageLimit)
                ).ToList();
            var messagesViewModel = _mapper.Map<IEnumerable<MessageViewModel>>(messages);

            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(messagesViewModel));
            await _cache.SetAsync($"messages-{notification.LobbyId}", serializedData, options, cancellationToken);
        }
    }
}
