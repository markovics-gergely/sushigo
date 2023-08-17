using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SpecialOrderCommand : ICardCommand<SpecialOrder>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddPoint _simpleAddPoint;

        public ClaimsPrincipal? User { get; set; }

        public SpecialOrderCommand(IUnitOfWork unitOfWork, ISimpleAddPoint simpleAddPoint)
        {
            _unitOfWork = unitOfWork;
            _simpleAddPoint = simpleAddPoint;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get special order card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.SpecialOrder && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Set calculated flag for every special order card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get card identity to copy
            var cardId = Guid.Parse(handCard.AdditionalInfo[Additional.CardIds]);

            // Get card entity to copy
            var card = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.Id == cardId,
                    transform: x => x.AsNoTracking()
                ).FirstOrDefault();

            if (card != null)
            {
                // Create the copied card
                var boardCard = new BoardCard
                {
                    CardType = card.CardType,
                    BoardId = card.BoardId,
                    GameId = card.GameId,
                    AdditionalInfo = card.AdditionalInfo,
                };
                _unitOfWork.BoardCardRepository.Insert(boardCard);
            }
            
            // Discard the special order card used
            _unitOfWork.HandCardRepository.Delete(handCard);
            await _unitOfWork.Save();
        }
    }
}
