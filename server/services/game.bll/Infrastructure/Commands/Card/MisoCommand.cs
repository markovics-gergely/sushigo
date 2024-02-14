using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MisoCommand : ICardCommand<MisoSoup>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly ISimpleAddPoint _simpleAddPoint;

        public MisoCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, ISimpleAddPoint simpleAddPoint)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
            _simpleAddPoint = simpleAddPoint;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            await _simpleAddPoint.CalculateEndRound(boardCard, 3);
            return new() { boardCard.Id };
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get miso cards played this turn
            var misoList = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == player.GameId && x.IsSelected && x.CardInfo.CardType == CardType.MisoSoup,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(HandCard.CardInfo)
                );

            // Remove card if there is more than 1 player who played miso this turn
            if (misoList.Count() > 1)
            {
                foreach(var miso in misoList)
                {
                    _unitOfWork.HandCardRepository.Delete(handCard);
                    await _unitOfWork.Save();
                }
            }
            // Add to board otherwise
            else
            {
                await _simpleAddToBoard.AddToBoard(player, handCard);
            }
        }
    }
}
