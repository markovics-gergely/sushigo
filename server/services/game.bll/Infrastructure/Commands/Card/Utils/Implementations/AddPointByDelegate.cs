using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;

namespace game.bll.Infrastructure.Commands.Card.Utils.Implementations
{
    /// <summary>
    /// Helper class for cards with simple point calculation
    /// </summary>
    public class AddPointByDelegate : IAddPointByDelegate
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddPointByDelegate(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Guid>> CalculateEndRound(BoardCard boardCard, CalculatePoint calculatePoint)
        {
            // Get card entities of the player with the same type
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == boardCard.BoardId && x.CardInfo.CardType == boardCard.CardInfo.CardType,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Get player entity of the board
            var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == boardCard.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // Calculate the points to add
            var points = calculatePoint(cards);

            // Add points to the player
            player.Points += points;
            _unitOfWork.PlayerRepository.Update(player);

            await _unitOfWork.Save();
            return cards.Select(c => c.Id).ToList();
        }
    }
}
