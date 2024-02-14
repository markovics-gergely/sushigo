using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.ViewModels;
using game.bll.Validators;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Implementations;
using shared.bll.Validators.Interfaces;
using shared.dal.Models.Types;
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
        private IValidator? _validator;

        public CardCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Find next player who can play after the turn
        /// And send end turn event if there is no player who can play
        /// </summary>
        /// <param name="game">Game to search in</param>
        /// <param name="user">User who played</param>
        /// <param name="cancellationToken"></param>
        /// <returns>ID of the next player if there is any</returns>
        private Guid FindNextAfterTurnOrEndTurn(Game game, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            // Get boards with after turn action
            var afterBoards = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == game.Id
                ).Where(a => a.CardInfo.CardType.HasAfterTurn()).Select(a => a.BoardId).ToList();

            // Get player ids who can play after the turn from board or hand
            var afterPlayers = _unitOfWork.PlayerRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id,
                includeProperties: nameof(Player.SelectedCardInfo)
            ).Where(x => afterBoards.Contains(x.BoardId) || x.SelectedCardInfo?.CardType.HasAfterTurnInHand() == true).Select(a => a.Id).ToList();
            
            // Get list of players in the sequence of the game
            var playerList = game.PlayerIds.ToList();

            // Get index of the actual player
            var index = playerList.IndexOf(user.GetPlayerIdFromJwt());

            var remainingPlayers = playerList.Skip(index + 1).ToList();
            foreach (var playerId in remainingPlayers)
            {
                // return if next node has any after turn action
                if (afterPlayers.Contains(playerId))
                {
                    return playerId;
                }
            }
            // If there is no player who can play after the turn set phase to end turn
            game.Phase = Phase.EndTurn;
            _mediator.Publish(new EndTurnEvent { GameId = game.Id }, cancellationToken);
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

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and in the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.Turn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(PlayCardCommand));
            }

            // Get hand card to play entity
            var handCard = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayCardDTO.HandCardId,
                    includeProperties: nameof(HandCard.CardInfo)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

            // Set selected card
            handCard.IsSelected = true;

            // Update card from dto params
            var cardinfo = handCard.CardInfo;
            request.PlayCardDTO.CardInfo.UpdateCardInfo(cardinfo);
            _unitOfWork.CardInfoRepository.Update(cardinfo);
            
            // Save updates
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Update(handCard);

            // Get command associated with card
            var command = _serviceProvider.GetCommand(handCard.CardInfo.CardType.GetClass());
            if (command == null) throw new EntityNotFoundException(nameof(command));

            await command.OnPlayCard(handCard);

            // Increment actual player to the player sitting next to
            // or set first player with after turn action
            if (game.FirstPlayerId != player.NextPlayerId)
            {
                game.ActualPlayerId = player.NextPlayerId;
            }
            else
            {
                // Load selected cards to players
                var players = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId
                );
                var playerMap = players.ToDictionary(p => p.HandId);

                // Get selected cards
                var selectedCards = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId && x.IsSelected,
                    includeProperties: nameof(HandCard.CardInfo)
                ).ToList();

                foreach (var card in selectedCards)
                {
                    playerMap[card.HandId].SelectedCardInfo = card.CardInfo;
                    _unitOfWork.PlayerRepository.Update(playerMap[card.HandId]);
                }

                // Get boards with after turn action
                var afterBoards = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId,
                    includeProperties: nameof(BoardCard.CardInfo)
                ).Where(a => a.CardInfo.CardType.HasAfterTurn()).Select(a => a.BoardId).ToList();

                // Get player ids who can play after the turn
                var afterPlayers = players.Where(x => afterBoards.Contains(x.BoardId) || x.SelectedCardInfo?.CardType.HasAfterTurnInHand() == true).Select(a => a.Id).ToList();
                
                // Change phase if there is any player who can still play
                if (afterPlayers.Any())
                {
                    game.Phase = Phase.AfterTurn;
                    game.ActualPlayerId = game.PlayerIds.FirstOrDefault(afterPlayers.Contains);
                }
                else
                {
                    game.Phase = Phase.EndTurn;
                    game.ActualPlayerId = game.FirstPlayerId;
                }
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);

            // If the turn is over send end turn event
            if (game.Phase == Phase.EndTurn)
            {
                await _mediator.Publish(new EndTurnEvent { GameId = game.Id }, cancellationToken);
            }
        }

        public async Task Handle(SkipAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and after the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.AfterTurn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(SkipAfterTurnCommand));
            }

            // Set next player who can still play or end turn
            var next = FindNextAfterTurnOrEndTurn(game, request.User, cancellationToken);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);
        }

        public async Task Handle(PlayAfterTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and after the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.AfterTurn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(PlayAfterTurnCommand));
            }

            CardType cardType;
            // Card was played from hand
            if (request.PlayAfterTurnDTO.IsHandCard)
            {
                // Search card entity used
                var handCard = _unitOfWork.HandCardRepository.Get(
                        transform: x => x.AsNoTracking(),
                        filter: x => x.Id == request.PlayAfterTurnDTO.HandOrBoardCardId,
                        includeProperties: nameof(HandCard.CardInfo)
                    ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

                cardType = handCard.CardInfo.CardType;
            }
            // Card was played from board
            else
            {
                // Search card entity used
                var boardCard = _unitOfWork.BoardCardRepository.Get(
                        transform: x => x.AsNoTracking(),
                        filter: x => x.Id == request.PlayAfterTurnDTO.HandOrBoardCardId,
                        includeProperties: nameof(BoardCard.CardInfo)
                    ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(BoardCard));

                cardType = boardCard.CardInfo.CardType;
            }

            // Get command associated with card
            var command = _serviceProvider.GetCommand(cardType.GetClass());
            if (command == null) throw new EntityNotFoundException(nameof(command));

            // Get player entity of the user
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // play the action of the card used
            await command.OnAfterTurn(player, request.PlayAfterTurnDTO);

            // Find next player
            var next = FindNextAfterTurnOrEndTurn(game, request.User, cancellationToken);
            game.ActualPlayerId = next;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);
        }
    }
}
