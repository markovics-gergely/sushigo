using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure
{
    public class GameCommandHandler :
        IRequestHandler<CreateGameCommand, GameViewModel>
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
            var cardList = new Queue<CardType>(Shuffle(request.CreateGameDTO.DeckType));
            var players = _mapper.Map<List<Player>>(request.CreateGameDTO.Players);
            var handSize = GetHandCount(players.Count);

            foreach (var player in players.OrderBy(x => Guid.NewGuid()).ToList())
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
            }
            deck.Cards = cardList.ToList();
            AddDeckInfo(request.CreateGameDTO.DeckType, deck);
            _unitOfWork.DeckRepository.Insert(deck);
            var game = new Game {
                DeckType = request.CreateGameDTO.DeckType,
                Name = request.CreateGameDTO.Name,
                Deck = deck,
                Players = players,
            };
            _unitOfWork.GameRepository.Insert(game);
            await _unitOfWork.Save();
            var actualId = game.Players.First().Id;
            game.FirstPlayerId = actualId;
            game.ActualPlayerId = actualId;
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.Save();
            return _mapper.Map<GameViewModel>(game);
        }

        private static List<CardType> Shuffle(DeckType deckType, int round = 0)
        {
            var cardList = new List<CardType>();
            deckType.GetCardTypes().ToList().ForEach(cardType =>
            {
                cardList.AddRange(Enumerable.Repeat(cardType, cardType.SushiType().GetCount()));
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

        private async Task CalculateEndRound(Guid gameId, ClaimsPrincipal? user = null)
        {
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == gameId,
                    transform: x => x.AsNoTracking()
                );
            while (cards.Any(c => !c.IsCalculated))
            {
                foreach (var card in cards.Where(c => !c.IsCalculated))
                {
                    var command = GetCommand(card.CardType.GetClass());
                    if (command != null)
                    {
                        command.User = user;
                        await command.CalculateEndRound(card);
                    }
                }
            }
            foreach (var card in cards)
            {

            }
            await _unitOfWork.Save();
        }

        private ICardCommand<TCard>? GetCommand<TCard>(TCard card) where TCard : CardTypeWrapper
        {
            return (ICardCommand<TCard>?)_serviceProvider.GetService(typeof(ICardCommand<>).MakeGenericType(card.GetType()));
        }
    }
}
