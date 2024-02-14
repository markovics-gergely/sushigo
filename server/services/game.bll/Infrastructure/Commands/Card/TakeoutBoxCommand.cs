using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TakeoutBoxCommand : ICardCommand<TakeoutBox>
    {
        private static readonly int POINT = 2;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddPoint _simpleAddPoint;

        public TakeoutBoxCommand(IUnitOfWork unitOfWork, ISimpleAddPoint simpleAddPoint)
        {
            _unitOfWork = unitOfWork;
            _simpleAddPoint = simpleAddPoint;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            await _simpleAddPoint.CalculateEndRound(boardCard, POINT);
            return new() { boardCard.Id };
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get card identities to turn into bento boxes
            var cardIds = handCard.CardInfo.CardIds
                ?? throw new ArgumentNullException(nameof(handCard));

            // Remove those cards from the board and create bentos
            foreach (var cardId in cardIds)
            {
                // Get the card to convert
                var boardCard = _unitOfWork.BoardCardRepository.Get(
                        filter: x => x.Id == cardId,
                        transform: x => x.AsNoTracking(),
                        includeProperties: nameof(BoardCard.CardInfo))
                    .FirstOrDefault() ?? throw new EntityNotFoundException(nameof(BoardCard));

                // Convert the card to a bento box
                var cardInfo = boardCard.CardInfo;
                cardInfo.Reset();
                cardInfo.CardType = CardType.TakeoutBox;
                cardInfo.CustomTag = CardTagType.CONVERTED;

                _unitOfWork.CardInfoRepository.Update(cardInfo);
            }

            // Discard the bento card used
            _unitOfWork.HandCardRepository.Delete(handCard);
            await _unitOfWork.Save();
        }
    }
}
