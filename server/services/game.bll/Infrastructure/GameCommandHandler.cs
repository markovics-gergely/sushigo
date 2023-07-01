using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.dal.Models;

namespace game.bll.Infrastructure
{
    public class GameCommandHandler :
        IRequestHandler<CreateGameCommand, GameViewModel>,
        IRequestHandler<CalculateEndTurnCommand>,
        IRequestHandler<CalculateEndRoundCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GameCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<GameViewModel> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var deck = new Deck();
            var cardList = new Queue<CardType>(Shuffle(request.CreateGameDTO.DeckType, 0, request.CreateGameDTO.Players.Count()));
            var players = _mapper.Map<List<Player>>(request.CreateGameDTO.Players).OrderBy(x => Guid.NewGuid()).ToList();
            var handSize = GetHandCount(players.Count);

            var lastGuid = new Guid();
            foreach (var player in players)
            {
                _unitOfWork.PlayerRepository.Insert(player);
                var hand = new Hand { Player = player };
                _unitOfWork.HandRepository.Insert(hand);
                for (int i = 0; i < handSize; i++)
                {
                    var card = cardList.Dequeue();
                    var handCard = new HandCard { CardType = card, Hand = hand };
                    _unitOfWork.HandCardRepository.Insert(handCard);
                    hand.Cards.Add(handCard);
                }
                var board = new Board { Player = player };
                _unitOfWork.BoardRepository.Insert(board);
                player.Board = board;
                player.Hand = hand;
                player.NextPlayerId = lastGuid;
                lastGuid = player.Id;
            }
            players.First().NextPlayerId = players.Last().Id;

            deck.Cards = cardList.ToList();
            AddDeckInfo(request.CreateGameDTO.DeckType, deck);
            _unitOfWork.DeckRepository.Insert(deck);
            var game = new Game {
                DeckType = request.CreateGameDTO.DeckType,
                Name = request.CreateGameDTO.Name,
                Deck = deck,
                Players = players,
                PlayerIds = players.Select(x => x.Id).ToList()
            };
            _unitOfWork.GameRepository.Insert(game);
            await _unitOfWork.Save();
            var actualId = game.Players.First().Id;
            game.FirstPlayerId = actualId;
            game.ActualPlayerId = actualId;
            _unitOfWork.GameRepository.Update(game);
            foreach (var player in players)
            {
                foreach (var card in player!.Hand!.Cards)
                {
                    card.GameId = game.Id;
                    _unitOfWork.HandCardRepository.Update(card);
                }
            }
            await _unitOfWork.Save();
            return _mapper.Map<GameViewModel>(game);
        }

        private static List<CardType> Shuffle(DeckType deckType, int round = 0, int count = 2)
        {
            var cardList = new List<CardType>();
            deckType.GetCardTypes().ToList().ForEach(cardType =>
            {
                cardList.AddRange(Enumerable.Repeat(cardType, cardType.SushiType().GetCount(round)));
            });
            return cardList.OrderBy(x => Guid.NewGuid()).ToList();
        }

        private static void AddDeckInfo(DeckType deckType, Deck deck)
        {
            var cardTypes = deckType.GetCardTypes();
            if (cardTypes.Contains(CardType.MakiRoll))
            {
                var points = string.Join(',', new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3 }.OrderBy(x => Guid.NewGuid()).ToList());
                deck.AdditionalInfo["maki"] = points;
            }
            if (cardTypes.Contains(CardType.Uramaki))
            {
                var points = string.Join(',', new int[] { 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5 }.OrderBy(x => Guid.NewGuid()).ToList());
                deck.AdditionalInfo["uramaki"] = points;
            }
            if (cardTypes.Contains(CardType.Onigiri))
            {
                var points = string.Join(',', new int[] { 0, 0, 1, 1, 2, 2, 3, 3 }.OrderBy(x => Guid.NewGuid()).ToList());
                deck.AdditionalInfo["onigiri"] = points;
            }
        }

        private static int GetHandCount(int playerCount)
        {
            if (playerCount < 4) return 10;
            if (playerCount < 6) return 9;
            if (playerCount < 8) return 8;
            return 7;
        }

        private ICardCommand<TCard>? GetCommand<TCard>(TCard card) where TCard : CardTypeWrapper
        {
            return (ICardCommand<TCard>?)_serviceProvider.GetService(typeof(ICardCommand<>).MakeGenericType(card.GetType()));
        }

        public async Task Handle(CalculateEndTurnCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            var handCards = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == request.User.GetGameIdFromJwt(),
                    transform: x => x.AsNoTracking()
                    ).Any();
            if (handCards)
            {
                var playerMap = game.Players.Select(p => new { Key = p.NextPlayerId, Value = p.HandId }).ToDictionary(p => p.Key, p => p.Value);
                foreach (var player in game.Players)
                {
                    player.HandId = playerMap[player.Id];
                    _unitOfWork.PlayerRepository.Update(player);
                }
                game.Phase = dal.Types.Phase.Turn;
            }
            else
            {
                game.Phase = dal.Types.Phase.EndRound;
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }

        public async Task Handle(CalculateEndRoundCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetGameIdFromJwt(),
                    includeProperties: nameof(Game.Players)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(CalculateEndRoundCommand));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            var deck = _unitOfWork.DeckRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(CalculateEndRoundCommand));
            if (deck == null) throw new EntityNotFoundException(nameof(deck));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == game.Id,
                    transform: x => x.AsNoTracking()
                );
            foreach (var card in cards.Where(c => !c.IsCalculated))
            {
                var command = GetCommand(card.CardType.GetClass());
                if (command != null)
                {
                    command.User = request.User;
                    await command.OnEndRound(card);
                }
            }
            if (game.Round != 2)
            {
                game.Round += 1;
                game.Phase = dal.Types.Phase.Turn;
                var cardList = new Queue<CardType>(Shuffle(game.DeckType, game.Round, game.PlayerIds.Count));
                var handSize = GetHandCount(game.PlayerIds.Count);
                foreach (var player in game.Players)
                {
                    for (int i = 0; i < handSize; i++)
                    {
                        var card = cardList.Dequeue();
                        var handCard = new HandCard { CardType = card, HandId = player.HandId };
                        _unitOfWork.HandCardRepository.Insert(handCard);
                    }
                }
                deck.Cards = cardList.ToList();
                AddDeckInfo(game.DeckType, deck);
                _unitOfWork.DeckRepository.Update(deck);
            }
            else
            {
                game.Phase = dal.Types.Phase.EndGame;
            }
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
        }
    }
}
