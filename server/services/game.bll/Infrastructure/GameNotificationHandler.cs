using AutoMapper;
using game.bll.Extensions;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using shared.bll.Exceptions;
using shared.dal.Repository.Interfaces;

namespace game.bll.Infrastructure
{
    public class GameNotificationHandler :
        INotificationHandler<RefreshGameEvent>,
        INotificationHandler<RefreshGameByIdEvent>,
        INotificationHandler<RemoveGameEvent>,
        INotificationHandler<EndRoundEvent>,
        INotificationHandler<EndTurnEvent>
    {
        private readonly ILogger _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GameNotificationHandler(ILogger<GameNotificationHandler> logger, ICacheRepository cacheRepository, IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Handle(RefreshGameEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Put($"game-{notification.GameViewModel.Id}", notification.GameViewModel, null, cancellationToken);
        }

        public async Task Handle(RemoveGameEvent notification, CancellationToken cancellationToken)
        {
            await _cacheRepository.Delete($"game-{notification.GameId}", cancellationToken);
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

        public async Task Handle(RefreshGameByIdEvent notification, CancellationToken cancellationToken)
        {
            var includeProperties = "Players.Board.Cards.CardInfo,Players.Hand.Cards.CardInfo,Players.SelectedCardInfo";

            // Get game entity for cache
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == notification.GameId,
                    includeProperties: includeProperties
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            var gameViewModel = _mapper.Map<GameViewModel>(gameForCache);
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = gameViewModel }, cancellationToken);

            // Refresh hands
            foreach (var player in gameForCache.Players)
            {
                if (player.Hand == null) continue;
                var handViewModel = new HandViewModel
                {
                    Cards = _mapper.Map<IEnumerable<HandCardViewModel>>(player.Hand.Cards)
                };
                await _mediator.Publish(new RefreshHandEvent { Hand = handViewModel, PlayerId = player.Id }, cancellationToken);
            }
        }
    }
}
