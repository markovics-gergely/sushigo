using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MakiCommand : ICardCommand<MakiRoll>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public MakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        /// <summary>
        /// Get points to add to the player
        /// </summary>
        /// <param name="placement"></param>
        /// <param name="playerCount"></param>
        /// <returns></returns>
        private static int CalculatePoint(int placement, int playerCount)
        {
            if (playerCount >= 6)
            {
                return placement switch
                {
                    0 => 6,
                    1 => 4,
                    2 => 2,
                    _ => 0
                };
            }
            return placement switch
            {
                0 => 6,
                1 => 3,
                _ => 0
            };
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get maki card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardInfo.CardType == CardType.MakiRoll,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Get count of players
            var playerCount = _unitOfWork.GameRepository.Get(
                    filter: x => x.Id == boardCard.GameId,
                    transform: x => x.AsNoTracking()
                ).First().PlayerIds.Count;

            // Group maki cards by board and points
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(x => new { BoardId = x.Key, Count = x.Select(xx => xx.CardInfo.Point).Sum() ?? 0 })
                .Where(x => x.Count > 0);

            // Get the top winner points
            var topPoints = boards.Select(x => x.Count).Distinct().OrderByDescending(x => x).Take(playerCount >= 6 ? 3 : 2);

            // Group winning boards by their points by inversing the groupping
            var topBoards = boards
                .Where(x => topPoints.Contains(x.Count))
                .GroupBy(x => x.Count)
                .ToDictionary(x => x.Key, x => x.Select(xx => xx.BoardId).ToList());

            // Iterate over each placement
            int placement = 0;
            foreach (var point in topPoints)
            {
                // Get list of boards of the placement
                var winnerBoards = topBoards[point];

                // Calculate point to add to the players
                var playerPoint = CalculatePoint(placement, playerCount);

                // Get player entities of the placement
                var players = _unitOfWork.PlayerRepository.Get(
                        filter: x => winnerBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();

                // Add point to the players
                foreach (var player in players)
                {
                    player.Points += playerPoint;
                    _unitOfWork.PlayerRepository.Update(player);
                }
            }
            await _unitOfWork.Save();
            return cards.Select(x => x.Id).ToList();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
