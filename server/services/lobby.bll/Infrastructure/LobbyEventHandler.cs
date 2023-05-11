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
    public class LobbyEventHandler :
        INotificationHandler<PlayerReadyEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;

        public LobbyEventHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache cache, IOptions<CacheSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _settings = settings.Value;
            _mapper = mapper;
        }

        public async Task Handle(PlayerReadyEvent notification, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(notification.LobbyViewModel));
            await _cache.SetAsync($"lobby-{notification.LobbyViewModel.Id}", serializedData, options, cancellationToken);
        }
    }
}
