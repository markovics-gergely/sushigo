using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MenuCommand : ICardCommand<Menu>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public ClaimsPrincipal? User { get; set; }

        public MenuCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get menu card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Menu && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Set calculated flag for every menu card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
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

            handCard.AdditionalInfo[Additional.MenuCards] = JsonConvert.SerializeObject(cardList);
            _unitOfWork.HandCardRepository.Update(handCard);
            await _unitOfWork.Save();
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            var selectedId = Guid.Parse(playAfterTurnDTO.AdditionalInfo[Additional.CardIds] ?? throw new ValidationErrorException(nameof(MenuCommand)));

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            var deck = _unitOfWork.DeckRepository.Get(
                transform: x => x.AsNoTracking(),
                    filter: x => x.Id == game.DeckId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Deck));

            var menuCard = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.Id == playAfterTurnDTO.HandCardId && x.HandId == player.HandId,
                    transform: x => x.AsNoTracking()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));
            if (!menuCard.AdditionalInfo.ContainsKey(Additional.MenuCards))
            {
                throw new ValidationErrorException(nameof(MenuCommand));
            }
            List<HandCard>? selectables = JsonConvert.DeserializeObject<List<HandCard>>(menuCard.AdditionalInfo[Additional.MenuCards]) ?? throw new ValidationErrorException(nameof(MenuCommand));
            if (!selectables.Any(x => x.Id == selectedId))
            {
                throw new ValidationErrorException(nameof(MenuCommand));
            }
            foreach (var selectable in selectables)
            {
                if (selectable.Id == selectedId)
                {
                    // Create the card to add to the board
                    var boardCard = new BoardCard
                    {
                        CardType = selectable.CardType,
                        BoardId = player.BoardId,
                        GameId = player.GameId,
                        AdditionalInfo = selectable.AdditionalInfo,
                    };
                    _unitOfWork.BoardCardRepository.Insert(boardCard);
                }
                else
                {
                    deck.Cards.Add(selectable.CardType);
                    if (selectable.AdditionalInfo.ContainsKey(Additional.Points))
                    {
                        deck.PushInfoItem(selectable.CardType, int.Parse(selectable.AdditionalInfo[Additional.Points]));
                    }
                }
            }
            player.SelectedCardId = null;
            player.SelectedCardType = null;
            _unitOfWork.PlayerRepository.Update(player);
            _unitOfWork.HandCardRepository.Delete(menuCard);
            _unitOfWork.DeckRepository.Update(deck);
            await _unitOfWork.Save();
        }
    }
}
