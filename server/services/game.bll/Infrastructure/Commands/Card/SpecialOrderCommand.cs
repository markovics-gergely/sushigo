using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SpecialOrderCommand : ICardCommand<SpecialOrder>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddPoint _simpleAddPoint;
        private readonly INoModification _noModification;

        public SpecialOrderCommand(IUnitOfWork unitOfWork, ISimpleAddPoint simpleAddPoint, INoModification noModification)
        {
            _unitOfWork = unitOfWork;
            _simpleAddPoint = simpleAddPoint;
            _noModification = noModification;
        }

        public Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            return Task.FromResult(_noModification.OnEndRound(boardCard));
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get card identity to copy
            var cardId = handCard.CardInfo.CardIds?.FirstOrDefault()
                ?? throw new EntityNotFoundException(nameof(SpecialOrder));

            // Get card entity to copy
            var card = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.Id == cardId,
                    transform: x => x.AsNoTracking()
                ).FirstOrDefault();

            if (card != null)
            {
                // Create the copied card
                var cardInfoCopy = (CardInfo) card.CardInfo.Clone();
                _unitOfWork.CardInfoRepository.Insert(cardInfoCopy);

                // Create the board card with the copied card
                var boardCard = new BoardCard
                {
                    BoardId = card.BoardId,
                    GameId = card.GameId,
                    CardInfo = cardInfoCopy,
                };
                _unitOfWork.BoardCardRepository.Insert(boardCard);
            }
            
            // Discard the special order card used
            _unitOfWork.HandCardRepository.Delete(handCard);
            await _unitOfWork.Save();
        }
    }
}
