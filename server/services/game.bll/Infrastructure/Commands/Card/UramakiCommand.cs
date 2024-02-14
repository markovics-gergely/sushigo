using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class UramakiCommand : ICardCommand<Uramaki>
    {
        private static readonly int MAX_CALC_COUNT = 3;
        private static readonly int TURN_GOAL = 10;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public UramakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
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
        private static int CalculatePoint(int placement) => placement switch
            {
                0 => 8,
                1 => 5,
                2 => 2,
                _ => 0,
            };

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get uramaki card entities of the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardInfo.CardType == CardType.Uramaki,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == boardCard.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            // Get how much player reached the uramaki goal before
            var uramaki = game.UramakiCalculatedCount;

            // If it needs to be calculated
            if (uramaki < MAX_CALC_COUNT)
            {
                // Goup cards by the boards of the players and their points
                var boards = cards
                    .GroupBy(c => c.BoardId)
                    .Select(c => new { BoardId = c.Key, Sum = c.Select(cc => cc.CardInfo.Point ?? 0).Sum() })
                    .Where(c => c.Sum > 0);

                // Get the top winner points
                var topPoints = boards.Select(x => x.Sum).Distinct().OrderByDescending(x => x).Take(MAX_CALC_COUNT - uramaki);

                // Group winning boards by their points by inversing the groupping
                var topBoards = boards
                    .Where(x => topPoints.Contains(x.Sum))
                    .GroupBy(x => x.Sum)
                    .ToDictionary(x => x.Key, x => x.Select(xx => xx.BoardId).ToList());

                // Iterate over each placement
                int placement = uramaki;
                foreach (var point in topPoints)
                {
                    // Get list of boards of the placement
                    var winnerBoards = topBoards[point];

                    // Calculate point to add to the players
                    var playerPoint = CalculatePoint(placement);

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
                    placement++;
                }
            }

            // Reset the count of the calculated uramaki
            game.UramakiCalculatedCount = 0;
            _unitOfWork.GameRepository.Update(game);

            await _unitOfWork.Save();
            return cards.Select(c => c.Id).ToList();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get uramaki card entities of the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == player.GameId && x.CardInfo.CardType == CardType.Uramaki,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );

            // Add uramaki card points from the board and played card
            var points = cards.Select(c => c.CardInfo.Point).Sum() + handCard.CardInfo.Point;
            
            // If the player reached the goal points
            if (points >= TURN_GOAL)
            {
                // Get game entity
                var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

                // Get how much player reached the uramaki goal before
                var uramaki = game.UramakiCalculatedCount;

                // Evaluate how much point to add
                var point = uramaki switch
                {
                    0 => 8,
                    1 => 5,
                    2 => 2,
                    _ => 0,
                };

                // Add points to the player
                player.Points += point;
                _unitOfWork.PlayerRepository.Update(player);

                // Remove uramaki cards used for the points
                foreach (var card in cards)
                {
                    _unitOfWork.BoardCardRepository.Delete(card);
                }
                _unitOfWork.HandCardRepository.Delete(handCard);

                // Increment the count of the players who reached the uramaki goal
                game.UramakiCalculatedCount++;
                _unitOfWork.GameRepository.Update(game);
                await _unitOfWork.Save();
            }
            // Otherwise simply place it on the board
            else
            {
                await _simpleAddToBoard.AddToBoard(player, handCard);
            }
        }
    }
}
