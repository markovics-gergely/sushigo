using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class UramakiCommand : ICardCommand<Uramaki>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

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

    public async Task OnEndRound(BoardCard boardCard)
        {
            // Get uramaki card entities of the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Uramaki && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == boardCard.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
            if (game == null) throw new EntityNotFoundException(nameof(game));

            // Get how much player reached the uramaki goal before
            game.AdditionalInfo.TryGetValue(CardType.Uramaki, out string? uramakiCount);
            var uramaki = int.Parse(uramakiCount ?? "0");

            // If it needs to be calculated
            if (uramaki < 3)
            {
                // Goup cards by the boards of the players and their points
                var boards = cards
                    .GroupBy(c => c.BoardId)
                    .Select(c => new { c.Key, Value = c.Select(cc => int.Parse(cc.AdditionalInfo[Additional.Points])).Sum() })
                    .Where(c => c.Value > 0);

                // Get the top winner points
                var topPoints = boards.Select(x => x.Value).Distinct().OrderByDescending(x => x).Take(3 - uramaki);

                // Group winning boards by their points by inversing the groupping
                var topBoards = boards
                    .Where(x => topPoints.Contains(x.Value))
                    .GroupBy(x => x.Value)
                    .ToDictionary(x => x.Key, x => x.Select(xx => xx.Key).ToList());

                // Iterate over each placement
                int placement = 0;
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
                }
            }

            // Set calculated flag for every uramaki card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get uramaki card entities of the player
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == player.BoardId && x.CardType == CardType.Uramaki,
                    transform: x => x.AsNoTracking()
                );

            // Add uramaki card points from the board and played card
            var points = cards.Select(c => int.Parse(c.AdditionalInfo[Additional.Points])).Sum() + int.Parse(handCard.AdditionalInfo[Additional.Points]);
            
            // If the player reached the goal points
            if (points >= 10)
            {
                // Get game entity
                var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
                if (game == null) throw new EntityNotFoundException(nameof(game));

                // Get how much player reached the uramaki goal before
                game.AdditionalInfo.TryGetValue(CardType.Uramaki, out string? uramakiCount);
                var uramaki = int.Parse(uramakiCount ?? "0");

                // Evaluate how much point to add
                var point = int.Parse(uramakiCount ?? "0") switch
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
                game.AdditionalInfo[CardType.Uramaki] = (uramaki + 1).ToString();
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
