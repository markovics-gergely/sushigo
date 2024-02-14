using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shared.bll.Exceptions;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MenuCommand : ICardCommand<Menu>
    {
        private static readonly int CARD_COUNT = 4;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly INoModification _noModification;


        public MenuCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, INoModification noModification)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
            _noModification = noModification;
        }

        public Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            return Task.FromResult(_noModification.OnEndRound(boardCard));
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }

        public async Task OnPlayCard(HandCard handCard)
        {
            // Get player entity
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.HandId == handCard.HandId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Get deck entity
            var deck = _unitOfWork.DeckRepository.Get(
                transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Deck));

            var cardList = new List<CardInfo>();
            var poppedCards = deck.Cards.Take(CARD_COUNT);

            foreach (var card in poppedCards)
            {
                // Create card entity
                var poppedCardInfo = new CardInfo
                {
                    Id = Guid.NewGuid(),
                    CardType = card.CardType,
                    Point = card.Point,
                    GameId = game.Id,
                };
                cardList.Add(poppedCardInfo);
            }
            _unitOfWork.DeckRepository.Update(deck);
            await _unitOfWork.Save();

            handCard.CardInfo.CustomTagString = JsonConvert.SerializeObject(cardList);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            var selectedId = playAfterTurnDTO.TargetCardId;

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Get deck entity
            var deck = _unitOfWork.DeckRepository.Get(
                transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Deck));

            // Get menu card entity played by the player
            var menuCard = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.Id == playAfterTurnDTO.HandOrBoardCardId && x.HandId == player.HandId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(HandCard.CardInfo)
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

            // Get the card info list from the menu card
            List<CardInfo> selectables = JsonConvert.DeserializeObject<List<CardInfo>>(menuCard.CardInfo.CustomTagString ?? "") 
                ?? throw new ValidationErrorException(nameof(MenuCommand));
            if (!selectables.Any(x => x.Id == selectedId))
            {
                throw new ValidationErrorException(nameof(MenuCommand));
            }

            // Iterate over the selectables to add the selected card to the board and the others to the deck
            foreach (var selectable in selectables)
            {
                if (selectable.Id == selectedId)
                {
                    // Create the card to add to the board
                    _unitOfWork.CardInfoRepository.Insert(selectable);

                    var boardCard = new BoardCard
                    {
                        BoardId = player.BoardId,
                        GameId = player.GameId,
                        CardInfo = selectable,
                    };
                    _unitOfWork.BoardCardRepository.Insert(boardCard);
                }
                else
                {
                    // Add the other cards to the deck
                    deck.Cards.Enqueue(new CardTypePoint
                    {
                        CardType = selectable.CardType,
                        Point = selectable.Point,
                    });
                }
            }
            _unitOfWork.HandCardRepository.Delete(menuCard);
            _unitOfWork.DeckRepository.Update(deck);
            await _unitOfWork.Save();
        }
    }
}
