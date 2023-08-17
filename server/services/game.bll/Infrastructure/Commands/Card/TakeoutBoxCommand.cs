using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TakeoutBoxCommand : ICardCommand<TakeoutBox>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddPoint _simpleAddPoint;

        public ClaimsPrincipal? User { get; set; }

        public TakeoutBoxCommand(IUnitOfWork unitOfWork, ISimpleAddPoint simpleAddPoint)
        {
            _unitOfWork = unitOfWork;
            _simpleAddPoint = simpleAddPoint;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            await _simpleAddPoint.CalculateEndRound(boardCard, 2);
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get card identities to turn into bento boxes
            var cardIds = handCard.AdditionalInfo[Additional.CardIds].Split(',').Select(Guid.Parse);

            // Remove those cards from the board and create bentos
            foreach (var cardId in cardIds)
            {
                _unitOfWork.BoardCardRepository.Delete(cardId);

                var boardCard = new BoardCard
                {
                    CardType = CardType.TakeoutBox,
                    BoardId = player.BoardId,
                    GameId = player.GameId,
                };
                boardCard.AdditionalInfo[Additional.Tagged] = "converted";
                _unitOfWork.BoardCardRepository.Insert(boardCard);
            }

            // Discard the bento card used
            _unitOfWork.HandCardRepository.Delete(handCard);
            await _unitOfWork.Save();
        }
    }
}
