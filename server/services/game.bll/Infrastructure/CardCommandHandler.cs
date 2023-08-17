using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.Commands.Card.Utils.Implementations;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.ViewModels;
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

        private Guid FindNextAfterTurn(Game game, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            // Get Players with after turn action
            var players = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == user.GetGameIdFromJwt() && x.CanPlayAfterTurn
                ).Select(p => p.Id).ToList();
            var list = new LinkedList<Guid>(game.PlayerIds);

            // Get actual players node
            var actual = list.Find(user.GetPlayerIdFromJwt());
            if (actual != null && list.Count > 0)
            {
                // Iterate next node
                var nextNode = actual.Next ?? list.First;
                while (nextNode != actual && nextNode != null)
                {
                    // return if next node has any after turn action
                    if (players.Contains(nextNode.Value))
                    {
                        return nextNode.Value;
                    }
                    nextNode = nextNode.Next ?? list.First;
                }
            }
            game.Phase = dal.Types.Phase.EndTurn;
            _mediator.Publish(new EndTurnEvent { GameId = game.Id, Principal = user }, cancellationToken);
            return game.FirstPlayerId;
        }

        public async Task Handle(PlayCardCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get player entity
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));
            if (player == null) throw new EntityNotFoundException(nameof(Player));

            // Get card to play entity
            var handCard = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayCardDTO.HandCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));
            if (handCard == null) throw new EntityNotFoundException(nameof(HandCard));

            // Set selected card
            player.SelectedCardId = handCard.Id;
            player.SelectedCardType = handCard.CardType;
            handCard.IsSelected = true;

            // Copy info from request to card info
            foreach (var pair in request.PlayCardDTO.AdditionalInfo)
            {
                handCard.AdditionalInfo.Add(pair.Key, pair.Value);
            }

            // Save updates
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId,
                    includeProperties: nameof(Game.Players) // For cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Increment actual player to the player sitting next to
            // or set first player with after turn action
            if (game.FirstPlayerId != player.NextPlayerId)
            {
                game.ActualPlayerId = player.NextPlayerId;
            }
            else
            {
                // Get player ids who can play after the turn
                var afterPlayers = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId && x.CanPlayAfterTurn
                ).Select(a => a.Id).ToList();

                // Change phase if there is any player who can still play
                if (afterPlayers.Any())
                {
                    game.Phase = dal.Types.Phase.AfterTurn;
                    game.ActualPlayerId = game.PlayerIds.FirstOrDefault(afterPlayers.Contains);
                }
                else
                {
                    game.Phase = dal.Types.Phase.EndTurn;
                    game.ActualPlayerId = game.FirstPlayerId;
                }
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = _mapper.Map<GameViewModel>(game) }, cancellationToken);

            // If the turn is over send end turn event
            if (game.Phase == dal.Types.Phase.EndTurn)
            {
                await _mediator.Publish(new EndTurnEvent { GameId = game.Id, Principal = request.User }, cancellationToken);
            }
        }

        public async Task Handle(SkipAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players) // For cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Set next player who can still play
            var next = FindNextAfterTurn(game, request.User, cancellationToken);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = _mapper.Map<GameViewModel>(game) }, cancellationToken);
        }

        public async Task Handle(PlayAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players) // For cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Search card entity used
            var boardCard = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayAfterTurnDTO.BoardCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(BoardCard));
            if (boardCard == null) throw new EntityNotFoundException(nameof(BoardCard));

            // Get command associated with card
            var command = _serviceProvider.GetCommand(boardCard.CardType.GetClass());
            if (command == null) throw new EntityNotFoundException(nameof(command));

            // Get player entity of the user
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));
            if (player == null) throw new EntityNotFoundException(nameof(Player));

            // play the action of the card used
            await command.OnAfterTurn(player, request.PlayAfterTurnDTO);

            // Find next player
            var next = FindNextAfterTurn(game, request.User, cancellationToken);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = _mapper.Map<GameViewModel>(game) }, cancellationToken);
        }
    }
}
