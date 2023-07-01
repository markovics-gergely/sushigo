using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Abstract;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure
{
    public class CardCommandHandler :
        IRequestHandler<PlayCardCommand>,
        IRequestHandler<SkipAfterTurnCommand>,
        IRequestHandler<PlayAfterTurnCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CardCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Handle(PlayCardCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            if (player == null) throw new EntityNotFoundException(nameof(player));
            var handCard = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayCardDTO.HandCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(PlayCardCommand));
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            player.SelectedCardId = request.PlayCardDTO.HandCardId;
            handCard.IsSelected = true;
            foreach (var pair in request.PlayCardDTO.AdditionalInfo)
            {
                handCard.AdditionalInfo.Add(pair.Key, pair.Value);
            }
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            if (game.FirstPlayerId != player.NextPlayerId)
            {
                game.ActualPlayerId = player.NextPlayerId;
            }
            else
            {
                var afterPlayers = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId && x.CanPlayAfterTurn
                ).Select(a => a.Id).ToList();
                if (afterPlayers.Any())
                {
                    game.Phase = dal.Types.Phase.AfterTurn;
                    game.ActualPlayerId = game.PlayerIds.FirstOrDefault(p => afterPlayers.Contains(p));
                }
                else
                {
                    game.Phase = dal.Types.Phase.EndTurn;
                    game.ActualPlayerId = game.FirstPlayerId;
                }
            }
            await _unitOfWork.Save(); 
        }

        public async Task Handle(SkipAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            var next = FindNextAfterTurn(game, request.User);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }

        private async Task CalculateEndTurn(Game game, Player player)
        {
            if (game.Phase == dal.Types.Phase.Turn && player.NextPlayerId == game.FirstPlayerId)
            {
                game.Phase = dal.Types.Phase.AfterTurn;
            }
            if (game.Phase == dal.Types.Phase.Turn)
            {
                game.ActualPlayerId = player.NextPlayerId;
            }
            else if (game.Phase == dal.Types.Phase.AfterTurn)
            {
                var players = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == game.Id && x.CanPlayAfterTurn
                ).Select(p => p.Id).ToList();
                var list = new LinkedList<Guid>(game.PlayerIds);
                var actual = list.Find(player.Id);
                Guid? next = null;
                if (actual != null && list.Count > 0)
                {
                    var nextNode = actual.Next ?? list.First;
                    while (nextNode != actual && nextNode != null) {
                        if (players.Contains(nextNode.Value))
                        {
                            next = nextNode.Value;
                            break;
                        }
                        nextNode = nextNode.Next ?? list.First;
                    }
                }
                if (next == null)
                {
                    game.ActualPlayerId = player.NextPlayerId;
                    game.Phase = dal.Types.Phase.EndTurn;
                } else
                {
                    game.ActualPlayerId = next.Value;
                }
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }
        private ICardCommand<TCard>? GetCommand<TCard>(TCard card) where TCard : CardTypeWrapper
        {
            return (ICardCommand<TCard>?) _serviceProvider.GetService(typeof(ICardCommand<>).MakeGenericType(card.GetType()));
        }
        private Guid FindNextAfterTurn(Game game, ClaimsPrincipal user)
        {
            var players = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == user.GetGameIdFromJwt() && x.CanPlayAfterTurn
                ).Select(p => p.Id).ToList();
            var list = new LinkedList<Guid>(game.PlayerIds);
            var actual = list.Find(user.GetPlayerIdFromJwt());
            if (actual != null && list.Count > 0)
            {
                var nextNode = actual.Next ?? list.First;
                while (nextNode != actual && nextNode != null)
                {
                    if (players.Contains(nextNode.Value))
                    {
                        return nextNode.Value;
                    }
                    nextNode = nextNode.Next ?? list.First;
                }
            }
            game.Phase = dal.Types.Phase.EndTurn;
            return game.FirstPlayerId;
        }

        public async Task Handle(PlayAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(PlayAfterTurnCommand));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            var boardCard = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayAfterTurnDTO.BoardCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(PlayAfterTurnCommand));
            if (boardCard == null) throw new EntityNotFoundException(nameof(boardCard));
            var command = GetCommand(boardCard.CardType.GetClass());
            if (command == null) throw new EntityNotFoundException(nameof(command));
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(PlayAfterTurnCommand));
            if (player == null) throw new EntityNotFoundException(nameof(player));
            await command.OnAfterTurn(player, request.PlayAfterTurnDTO);
            var next = FindNextAfterTurn(game, request.User);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }
    }
}
