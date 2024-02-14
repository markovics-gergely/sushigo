using AutoMapper;
using game.bll.Extensions;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.ViewModels;
using game.bll.Validators;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Hangfire;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Implementations;
using shared.bll.Validators.Interfaces;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure
{
    public class GameCommandHandler :
        IRequestHandler<CreateGameCommand, GameViewModel>,
        IRequestHandler<ProceedEndTurnCommand>,
        IRequestHandler<ProceedEndRoundCommand>,
        IRequestHandler<ProceedEndGameCommand>,
        IRequestHandler<RemoveGameCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _endpoint;
        private readonly IBackgroundJobClient _client;
        private IValidator? _validator;

        public GameCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator, IPublishEndpoint endpoint, IBackgroundJobClient client)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
            _endpoint = endpoint;
            _client = client;
        }

        public async Task<GameViewModel> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            // Create a game
            var game = new Game
            {
                DeckType = request.CreateGameDTO.DeckType,
                Name = request.CreateGameDTO.Name,
                Phase = Phase.Turn
            };
            _unitOfWork.GameRepository.Insert(game);

            // Shuffle cards from deck type and count of players
            var cardList = request.CreateGameDTO.DeckType.GetShuffledCards(0, request.CreateGameDTO.Players.Count());

            // Set remaining cards to a deck
            var deck = new Deck
            {
                DeckType = request.CreateGameDTO.DeckType,
                Cards = cardList,
            };
            _unitOfWork.DeckRepository.Insert(deck);

            // Shuffle players
            var players = _mapper.Map<List<Player>>(request.CreateGameDTO.Players).OrderBy(x => Guid.NewGuid()).ToList();

            // Get hand size based on count of players
            var handSize = Game.GetHandCount(players.Count);

            // Create players and cards in their hands
            var lastGuid = new Guid();
            foreach (var player in players)
            {
                var hand = new Hand();
                _unitOfWork.HandRepository.Insert(hand);

                // Get cards from shuffled deck
                for (int i = 0; i < handSize; i++)
                {
                    var card = cardList.Dequeue();

                    // Create card entity
                    var cardInfo = new CardInfo
                    {
                        CardType = card.CardType,
                        Point = card.Point,
                        GameId = game.Id,
                    };
                    _unitOfWork.CardInfoRepository.Insert(cardInfo);

                    // Create connection between hand and card entity
                    var handCard = new HandCard { Hand = hand, GameId = game.Id, CardInfo = cardInfo };
                    _unitOfWork.HandCardRepository.Insert(handCard);
                }

                // Create board
                var board = new Board { GameId = game.Id };
                _unitOfWork.BoardRepository.Insert(board);

                // Set player properties
                player.Board = board;
                player.Hand = hand;
                player.NextPlayerId = lastGuid;
                player.GameId = game.Id;
                _unitOfWork.PlayerRepository.Insert(player);

                // Increment next player node
                lastGuid = player.Id;
            }
            // Attach last node to first
            players.First().NextPlayerId = players.Last().Id;

            game.PlayerIds = players.Select(x => x.Id).ToList();
            game.Deck = deck;

            // Set actual player indicator
            var actualId = game.PlayerIds.First();
            game.FirstPlayerId = actualId;
            game.ActualPlayerId = actualId;

            await _unitOfWork.Save();

            // Send game created to user management
            var lobbyId = request.User?.GetUserLobbyFromJwt();
            await _endpoint.Publish(new GameJoinedDTO
            {
                LobbyId = string.IsNullOrEmpty(lobbyId) ? Guid.Empty : Guid.Parse(lobbyId),
                Users = players.Select(p => new GameJoinedPlayerDTO
                {
                    UserId = p.UserId,
                    PlayerId = p.Id
                }),
                GameId = game.Id
            }, cancellationToken);
            return _mapper.Map<GameViewModel>(game);
        }

        public async Task Handle(ProceedEndTurnCommand request, CancellationToken cancellationToken)
        {
            var gameId = request.GameId ?? request.User!.GetGameIdFromJwt();

            // Delete end turn job
            MediatorExtensions.Delete($"end-turn-{gameId}");

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == gameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate player if it is not a job
            _validator = request.IsJob ? new PhaseValidator(game, Phase.EndTurn) : new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.EndTurn)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndTurnCommand));
            }

            // Get players of the game
            var players = _unitOfWork.PlayerRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                );

            // Get played cards by ordered by hand id
            var selectedCards = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == game.Id && x.IsSelected,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(HandCard.CardInfo)
                    ).ToDictionary(x => x.HandId);

            // Iterate over the players
            foreach (var player in players)
            {
                // Get selected card
                selectedCards.TryGetValue(player.HandId, out var selected);
                if (selected == null) continue;

                // Get command associated with card
                var command = _serviceProvider.GetCommand(selected.CardInfo.CardType.GetClass());
                if (command == null) throw new EntityNotFoundException(nameof(command));

                // play the action of the card used
                await command.OnEndTurn(player, selected);

                // Remove selected card from the player
                player.SelectedCardInfoId = null;
                _unitOfWork.PlayerRepository.Update(player);

                await _unitOfWork.Save();
            }

            // Check if there is any hand to swap
            var handCards = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                    ).Any();

            // If there are hand cards, swap hands
            if (handCards)
            {
                // Swap hands with each other
                var playerMap = players.Select(p => new { Key = p.NextPlayerId, Value = p.HandId }).ToDictionary(p => p.Key, p => p.Value);
                foreach (var player in players)
                {
                    player.HandId = playerMap[player.Id];
                    _unitOfWork.PlayerRepository.Update(player);
                }
                await _unitOfWork.Save();
                game.Phase = Phase.Turn;
            }
            else
            {
                // Otherwise it is the end of the round
                game.Phase = Phase.EndRound;
            }
            _unitOfWork.GameRepository.Update(game);

            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);

            // Initiate end round event if it is needed
            if (game.Phase == Phase.EndRound)
            {
                await _mediator.Publish(new EndRoundEvent
                {
                    GameId = game.Id
                }, cancellationToken);
            }
        }

        public async Task Handle(ProceedEndRoundCommand request, CancellationToken cancellationToken)
        {
            var gameId = request.GameId ?? request.User!.GetGameIdFromJwt();

            // Delete end round job
            MediatorExtensions.Delete($"end-round-{gameId}");

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == gameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate player if it is not a job
            _validator = request.IsJob ? new PhaseValidator(game, Phase.EndRound) : new AndCondition(
                new ActualPlayerValidator(game, request.User),
                new PhaseValidator(game, Phase.EndRound)
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndRoundCommand));
            }
            
            // Get deck entity
            var deck = _unitOfWork.DeckRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Deck));

            // Get cards on the board
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)).ToList();
            
            // List of calculated cards during iteration
            var calculatedCards = new List<Guid>();

            // Iterate over cards that are not already calculated
            // Filter out dessert cards
            foreach (var card in cards.Where(c => c.CardInfo.CardType.SushiType() != SushiType.Dessert))
            {
                // Skip this card if it was already calculated
                if (calculatedCards.Contains(card.Id)) { continue; }

                // Calculate end round action through the command of the card type
                var command = _serviceProvider.GetCommand(card.CardInfo.CardType.GetClass());
                if (command != null)
                {
                    calculatedCards.AddRange(await command.OnEndRound(card));
                }
            }

            // Delete board cards that are not dessert
            _unitOfWork.BoardCardRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id,
                includeProperties: nameof(BoardCard.CardInfo)
            ).Where(c => c.CardInfo.CardType.SushiType() != SushiType.Dessert).ToList().ForEach(_unitOfWork.BoardCardRepository.Delete);

            // If it was the last round end the game
            if (game.IsOver())
            {
                game.Phase = Phase.EndGame;
            }
            // Otherwise start the next round
            else
            {
                // Start the next round
                game.Round += 1;
                game.Phase = Phase.Turn;

                // Reshuffle the deck
                var cardList = deck.DeckType.GetShuffledCards(game.Round, game.PlayerIds.Count);
                var handSize = game.GetHandCount();

                // Get players of the game
                var players = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.GameId == game.Id,
                        transform: x => x.AsNoTracking()
                    );

                // Iterate over the players
                foreach (var player in players)
                {
                    // Restore the hand to the full size
                    for (int i = 0; i < handSize; i++)
                    {
                        // Add a card to the player from the top of the deck
                        var card = cardList.Dequeue();

                        // Create card entity
                        var cardInfo = new CardInfo
                        {
                            CardType = card.CardType,
                            Point = card.Point,
                            GameId = game.Id,
                        };
                        _unitOfWork.CardInfoRepository.Insert(cardInfo);

                        // Create connection between hand and card entity
                        var handCard = new HandCard { HandId = player.HandId, GameId = game.Id, CardInfo = cardInfo };
                        _unitOfWork.HandCardRepository.Insert(handCard);
                    }
                }

                // Set remaining cards to deck
                deck.Cards = cardList;

                _unitOfWork.DeckRepository.Update(deck);
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);
        }

        public async Task Handle(RemoveGameCommand request, CancellationToken cancellationToken)
        {
            // Get game entity with players
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate if first player started the remove
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(RemoveGameCommand));
            }

            // Delete cards
            _unitOfWork.CardInfoRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id
            ).ToList().ForEach(_unitOfWork.CardInfoRepository.Delete);

            // Delete board cards
            _unitOfWork.BoardCardRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id
            ).ToList().ForEach(_unitOfWork.BoardCardRepository.Delete);

            // Delete hand cards
            _unitOfWork.HandCardRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id
            ).ToList().ForEach(_unitOfWork.HandCardRepository.Delete);

            // Delete players, boards and hands
            foreach (var player in game.Players)
            {
                _unitOfWork.PlayerRepository.Delete(player);
                _unitOfWork.HandRepository.Delete(player.HandId);
                _unitOfWork.BoardRepository.Delete(player.BoardId);
            }

            // Delete game and deck
            _unitOfWork.GameRepository.Delete(game);
            _unitOfWork.DeckRepository.Delete(game.DeckId);
            await _unitOfWork.Save();

            // Send remove event to cache handler
            await _mediator.Publish(new RemoveGameEvent { GameId = game.Id }, cancellationToken);

            // Send game over event to user container
            var isOver = game.Phase == Phase.Result;

            // Calculate distinct placements by points
            var placement = game.Players.Select(p => p.Points).Distinct().OrderDescending().ToList();
            foreach (var player in game.Players)
            {
                await _endpoint.Publish(new GameEndDTO
                {
                    Points = isOver ? player.Points : 0,
                    // Calculate placement from player points if game is over
                    Placement = isOver ? placement.FindIndex(p => p == player.Points) + 1 : 0,
                    UserId = player.UserId,
                    GameName = game.Name,
                }, cancellationToken);
            }
        }

        public async Task Handle(ProceedEndGameCommand request, CancellationToken cancellationToken)
        {
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Validate if first player played
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndRoundCommand));
            }

            // Get cards on the board
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );

            // List of calculated cards during iteration
            var calculatedCards = new List<Guid>();

            // Iterate over dessert cards to calculate (there should be only dessert cards)
            foreach (var card in cards.Where(c => c.CardInfo.CardType.SushiType() == SushiType.Dessert))
            {
                // Skip this card if it was already calculated
                if (calculatedCards.Contains(card.Id)) { continue; }

                // Calculate end game action through the command of the card type
                var command = _serviceProvider.GetCommand(card.CardInfo.CardType.GetClass());
                if (command != null)
                {
                    calculatedCards.AddRange(await command.OnEndGame(card));
                }
            }

            // Set game phase to result page
            game.Phase = Phase.Result;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Delete remaining board cards
            _unitOfWork.BoardCardRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id
            ).ToList().ForEach(_unitOfWork.BoardCardRepository.Delete);
            await _unitOfWork.Save();

            // Send refresh to users and cache
            await _mediator.Publish(new RefreshGameByIdEvent { GameId = game.Id }, cancellationToken);
        }
    }
}
