using AutoMapper;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.dal.Repository.Interfaces;

namespace lobby.bll.Infrastructure
{
    public class LobbyEventHandler :
        INotificationHandler<RefreshLobbyEvent>,
        INotificationHandler<UpdateDeckTypeEvent>,
        INotificationHandler<PlayerReadyEvent>,
        INotificationHandler<AddLobbyEvent>,
        INotificationHandler<RemoveLobbyEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;

        public LobbyEventHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheRepository cacheRepository)
        {
            _unitOfWork = unitOfWork;
            _cacheRepository = cacheRepository;
            _mapper = mapper;
        }

        public async Task Handle(PlayerReadyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobby(notification.LobbyViewModel, cancellationToken);
        }

        public async Task Handle(RefreshLobbyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobby(notification.Lobby, cancellationToken);
        }

        public async Task Handle(AddLobbyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobbies(cancellationToken);
            await RefreshLobby(notification.Lobby, cancellationToken);
        }

        public async Task Handle(RemoveLobbyEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobbies(cancellationToken);
            await _cacheRepository.Delete($"lobby-{notification.LobbyId}", cancellationToken);
        }

        private async Task RefreshLobby(LobbyViewModel lobby, CancellationToken cancellationToken)
        {
            await _cacheRepository.Put($"lobby-{lobby.Id}", lobby, null, cancellationToken);
        }

        private async Task RefreshLobbies(CancellationToken cancellationToken)
        {
            var lobbies = _unitOfWork.LobbyRepository.Get(
                    transform: x => x.AsNoTracking().OrderByDescending(l => l.Created)
                ).ToList();
            var lobbiesViewModel = _mapper.Map<IEnumerable<LobbyItemViewModel>>(lobbies);
            await _cacheRepository.Put("lobbies", lobbiesViewModel, null, cancellationToken);
        }

        public async Task Handle(UpdateDeckTypeEvent notification, CancellationToken cancellationToken)
        {
            await RefreshLobby(notification.LobbyViewModel, cancellationToken);
        }
    }
}
