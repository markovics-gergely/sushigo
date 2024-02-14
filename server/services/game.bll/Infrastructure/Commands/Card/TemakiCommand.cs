using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TemakiCommand : ICardCommand<Temaki>
    {
        private static readonly int POINT = 4;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public TemakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get temaki card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardInfo.CardType == CardType.Temaki,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Group cards by board and evaluate points
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(c => new { BoardId = c.Key, Count = c.Count() });

            // Get top points
            var maxCount = boards.MaxBy(b => b.Count)?.Count;

            // Find player entities with the top points
            var maxBoards = boards.Where(b => b.Count == maxCount).Select(b => b.BoardId).ToList();
            var maxPlayers = _unitOfWork.PlayerRepository.Get(
                        filter: x => maxBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();

            // Add points to each top earner
            foreach (var player in maxPlayers)
            {
                player.Points += POINT;
                _unitOfWork.PlayerRepository.Update(player);
            }

            // Get bottom points
            var minCount = boards.MinBy(b => b.Count)?.Count;
            // Find player entities with the bottom points
            var minBoards = boards.Where(b => b.Count == minCount).Select(b => b.BoardId).ToList();
            var minPlayers = _unitOfWork.PlayerRepository.Get(
                        filter: x => minBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();

            // Remove points for each bottom earner
            foreach (var player in minPlayers)
            {
                player.Points -= POINT;
                _unitOfWork.PlayerRepository.Update(player);
            }

            await _unitOfWork.Save();
            return cards.Select(c => c.Id).ToList();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
