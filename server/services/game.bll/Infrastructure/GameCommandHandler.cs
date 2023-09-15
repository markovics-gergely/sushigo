using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.ViewModels;
using game.bll.Validators;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using shared.dal.Models;
using System;

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
        private IValidator? _validator;

        public GameCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator, IPublishEndpoint endpoint)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
            _endpoint = endpoint;
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
                DeckType = request.CreateGameDTO.DeckType
            };

            // Generate additional card informations
            deck.AddDeckInfo();

            // Shuffle players
            var players = _mapper.Map<List<Player>>(request.CreateGameDTO.Players).OrderBy(x => Guid.NewGuid()).ToList();
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
                    var handCard = new HandCard { CardType = card, Hand = hand, GameId = game.Id };
                    var info = deck.PopInfoItem(card);
                    if (info != null)
                    {
                        handCard.AdditionalInfo[Additional.Points] = info?.ToString() ?? "";
                    }
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

            deck.Cards = cardList.ToList();
            _unitOfWork.DeckRepository.Insert(deck);

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
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if first player played
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndTurnCommand));
            }

            // Get players of the game
            var players = _unitOfWork.PlayerRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                );

            // Get played cards by hand
            var selectedCards = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == game.Id && x.IsSelected,
                    transform: x => x.AsNoTracking()
                    ).ToDictionary(x => x.HandId);

            foreach (var player in players)
            {
                // Get selected card
                var selected = selectedCards[player.HandId];

                // Get command associated with card
                var command = _serviceProvider.GetCommand(selected.CardType.GetClass());
                if (command == null) throw new EntityNotFoundException(nameof(command));

                // play the action of the card used
                await command.OnEndTurn(player, selected);

                // Remove selected card from the player
                player.SelectedCardType = null;
                player.SelectedCardId = null;
                _unitOfWork.PlayerRepository.Update(player);

                await _unitOfWork.Save();
            }

            // Check if there is any hand to swap
            var handCards = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                    ).Any();
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

            // Get game entity
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            var gameViewModel = _mapper.Map<GameViewModel>(gameForCache);
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = gameViewModel }, cancellationToken);

            // Initiate end round event if it is needed
            if (game.Phase == Phase.EndRound)
            {
                await _mediator.Publish(new EndRoundEvent
                {
                    GameId = game.Id,
                    Principal = request.User!
                }, cancellationToken);
            }
        }

        public async Task Handle(ProceedEndRoundCommand request, CancellationToken cancellationToken)
        {
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if first player played
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndRoundCommand));
            }

            // Get deck entity
            var deck = _unitOfWork.DeckRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game.Deck));
            if (deck == null) throw new EntityNotFoundException(nameof(Game.Deck));

            // Get cards on the board
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                );

            // Iterate over cards that are not already calculated
            foreach (var card in cards.Where(c => !c.IsCalculated && c.CardType.SushiType() != SushiType.Dessert))
            {
                // Calculate end round action through the command of the card type
                var command = _serviceProvider.GetCommand(card.CardType.GetClass());
                if (command != null)
                {
                    await command.OnEndRound(card);
                }
            }

            // Delete board cards
            _unitOfWork.BoardCardRepository.Get(
                transform: x => x.AsNoTracking(),
                filter: x => x.GameId == game.Id
            ).Where(c => c.CardType.SushiType() != SushiType.Dessert).ToList().ForEach(_unitOfWork.BoardCardRepository.Delete);

            // If it was the last round end the game
            if (game.IsOver())
            {
                game.Phase = Phase.EndGame;
            }
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
                        var handCard = new HandCard { CardType = card, HandId = player.HandId, GameId = game.Id };
                        var info = deck.PopInfoItem(card);
                        if (info != null)
                        {
                            handCard.AdditionalInfo[Additional.Points] = info?.ToString() ?? "";
                        }
                        _unitOfWork.HandCardRepository.Insert(handCard);
                    }
                }

                // Set remaining cards to deck
                deck.Cards = cardList.ToList();

                // Generate additional card informations
                deck.AddDeckInfo();
                _unitOfWork.DeckRepository.Update(deck);
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Get game entity
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            var gameViewModel = _mapper.Map<GameViewModel>(gameForCache);
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = gameViewModel }, cancellationToken);
        }

        public async Task Handle(RemoveGameCommand request, CancellationToken cancellationToken)
        {
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(EndRoundEvent));

            // Validate if first player removed
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(RemoveGameCommand));
            }

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

            _unitOfWork.GameRepository.Delete(game);
            _unitOfWork.DeckRepository.Delete(game.DeckId);
            await _unitOfWork.Save();

            // Send remove event to cache handler
            await _mediator.Publish(new RemoveGameEvent { GameId = game.Id }, cancellationToken);

            // Send over event to user container
            var isOver = game.Phase == Phase.Result;
            foreach (var player in game.Players)
            {
                await _endpoint.Publish(new GameEndDTO
                {
                    Points = isOver ? player.Points : 0,
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
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Validate if first player played
            _validator = new FirstPlayerValidator(game, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(ProceedEndRoundCommand));
            }

            // Get cards on the board
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                );

            // Iterate over dessert cards to calculate
            foreach (var card in cards.Where(c => c.CardType.SushiType() == SushiType.Dessert))
            {
                // Calculate end game action through the command of the card type
                var command = _serviceProvider.GetCommand(card.CardType.GetClass());
                if (command != null)
                {
                    await command.OnEndGame(card);
                }
            }
            game.Phase = Phase.Result;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();

            // Get game entity
            var gameForCache = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: "Players.Board.Cards" // for cache
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));
            if (game == null) throw new EntityNotFoundException(nameof(Game));

            // Send refresh to users and cache
            var gameViewModel = _mapper.Map<GameViewModel>(gameForCache);
            await _mediator.Publish(new RefreshGameEvent { GameViewModel = gameViewModel }, cancellationToken);
        }
    }
}
