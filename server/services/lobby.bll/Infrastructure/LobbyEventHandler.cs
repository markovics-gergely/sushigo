using AutoMapper;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using shared.bll.Settings;
using System.Text;

namespace lobby.bll.Infrastructure
{
    public class LobbyEventHandler :
        INotificationHandler<PlayerReadyEvent>,
        INotificationHandler<AddLobbyEvent>,
        INotificationHandler<RemoveLobbyEvent>
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
            await RefreshLobby(notification.LobbyViewModel, cancellationToken);
        }

        public async Task Handle(AddLobbyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobbies(cancellationToken);
        }

        public async Task Handle(RemoveLobbyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobbies(cancellationToken);
        }

        private async Task RefreshLobby(LobbyViewModel lobby, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(lobby));
            await _cache.SetAsync($"lobby-{lobby.Id}", serializedData, options, cancellationToken);
        }

        private async Task RefreshLobbies(CancellationToken cancellationToken)
        {
            var lobbies = _unitOfWork.LobbyRepository.Get(
                    transform: x => x.AsNoTracking().OrderByDescending(l => l.Created)
                ).ToList();
            var lobbiesViewModel = _mapper.Map<IEnumerable<LobbyItemViewModel>>(lobbies);
            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(lobbiesViewModel));
            await _cache.SetAsync("lobbies", serializedData, options, cancellationToken);
        }
    }
}
