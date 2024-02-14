using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;

namespace game.bll.Infrastructure.Commands.Card.Utils.Implementations
{
    public class SimpleAddPoint : ISimpleAddPoint
    {
        private readonly IUnitOfWork _unitOfWork;
        public SimpleAddPoint(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CalculateEndRound(BoardCard boardCard, int point)
        {
            // Get player entity of the board
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTrackingWithIdentityResolution(),
                    filter: x => x.BoardId == boardCard.BoardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // Add points to the player
            player.Points += point;
            _unitOfWork.PlayerRepository.Update(player);

            await _unitOfWork.Save();
        }
    }
}
