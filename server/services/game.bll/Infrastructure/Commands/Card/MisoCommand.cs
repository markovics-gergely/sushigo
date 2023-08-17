using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MisoCommand : ICardCommand<MisoSoup>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly ISimpleAddPoint _simpleAddPoint;

        public ClaimsPrincipal? User { get; set; }

        public MisoCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, ISimpleAddPoint simpleAddPoint)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
            _simpleAddPoint = simpleAddPoint;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            await _simpleAddPoint.CalculateEndRound(boardCard, 3);
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get count of miso cards played this turn
            var misoCount = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == player.GameId && x.IsSelected && x.CardType == CardType.MisoSoup,
                    transform: x => x.AsNoTracking()
                ).Count();

            // Remove card if there is more than 1 player who played miso this turn
            if (misoCount > 1)
            {
                _unitOfWork.HandCardRepository.Delete(handCard);
                await _unitOfWork.Save();
            }
            // Add to board otherwise
            else
            {
                await _simpleAddToBoard.AddToBoard(player, handCard);
            }
        }
    }
}
