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
using Newtonsoft.Json;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Implementations;
using shared.bll.Validators.Interfaces;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure
{
    public class CardCommandHandler :
        IRequestHandler<PlayCardCommand>,
        IRequestHandler<SkipAfterTurnCommand>,
        IRequestHandler<PlayAfterTurnCommand>,
        IRequestHandler<PlayMenuCardCommand>
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

        private Guid FindNextAfterTurn(Game game, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var afterBoards = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == game.Id
                ).Where(a => a.CardType.HasAfterTurn()).Select(a => a.BoardId).ToList();

            // Get player ids who can play after the turn
            var afterPlayers = new List<Guid>();
            if (afterBoards.Any())
            {
                afterPlayers = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => afterBoards.Contains(x.BoardId)
                ).Select(a => a.Id).ToList();
            }
            var playerList = game.PlayerIds.ToList();
            var actualId = user.GetPlayerIdFromJwt();
            try
            {
                var subList = playerList.GetRange(playerList.IndexOf(actualId) + 1, playerList.Count - playerList.IndexOf(actualId) - 1);
                foreach (var playerId in subList)
                {
                    // return if next node has any after turn action
                    if (afterPlayers.Contains(playerId))
                    {
                        return playerId;
                    }
                }
            }
            catch { }
            game.Phase = dal.Types.Phase.EndTurn;
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
            if (player == null) throw new EntityNotFoundException(nameof(Player));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and in the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.Turn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(PlayCardCommand));
            }

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
                handCard.AdditionalInfo.TryAdd(pair.Key, pair.Value);
            }

            // Save updates
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();

            // Increment actual player to the player sitting next to
            // or set first player with after turn action
            if (game.FirstPlayerId != player.NextPlayerId)
            {
                game.ActualPlayerId = player.NextPlayerId;
            }
            else
            {
                var afterBoards = _unitOfWork.BoardCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.GameId == player.GameId
                ).Where(a => a.CardType.HasAfterTurn()).Select(a => a.BoardId).ToList();

                // Get player ids who can play after the turn
                var afterPlayers = new List<Guid>();
                if (afterBoards.Any())
                {
                    afterPlayers = _unitOfWork.PlayerRepository.Get(
                        transform: x => x.AsNoTracking(),
                        filter: x => afterBoards.Contains(x.BoardId)
                    ).Select(a => a.Id).ToList();
                }
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

            // Get game entity
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId,
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = _mapper.Map<GameViewModel>(gameForCache) }, cancellationToken);

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
                    filter: x => x.Id == request.User.GetGameIdFromJwt(),
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and after the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.AfterTurn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(SkipAfterTurnCommand));
            }

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
                    filter: x => x.Id == request.User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if actual player played and after the turn
            _validator = new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.AfterTurn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(PlayAfterTurnCommand));
            }

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

            // Get game entity
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId,
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = _mapper.Map<GameViewModel>(gameForCache) }, cancellationToken);
        }

        public async Task Handle(PlayMenuCardCommand request, CancellationToken cancellationToken)
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

            // Get card to play entity
            var handCard = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayCardDTO.HandCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

            // Set selected card
            player.SelectedCardId = handCard.Id;
            player.SelectedCardType = handCard.CardType;
            handCard.IsSelected = true;

            // Save updates
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();

            var deck = _unitOfWork.DeckRepository.Get(
                transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Deck));

            var cardList = new List<HandCard>();
            var poppedCards = deck.Cards.Take(4);
            foreach (var card in poppedCards)
            {
                var info = deck.PopInfoItem(card);
                var poppedCard = new HandCard
                {
                    CardType = card,
                    Id = Guid.NewGuid(),
                };
                if (info != null)
                {
                    poppedCard.AdditionalInfo[Additional.Points] = info?.ToString() ?? "";
                }
                cardList.Add(poppedCard);
            }
            _unitOfWork.DeckRepository.Update(deck);
            await _unitOfWork.Save();

            game.Phase = Phase.MenuSelect;
            handCard.AdditionalInfo[Additional.MenuCards] = JsonConvert.SerializeObject(cardList);
            _unitOfWork.GameRepository.Update(game);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();
        }
    }
}
