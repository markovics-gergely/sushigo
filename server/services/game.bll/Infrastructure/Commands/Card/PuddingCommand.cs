using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class PuddingCommand : ICardCommand<Pudding>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public PuddingCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get pudding card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Pudding && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Set calculated flag for every pudding card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndGame(BoardCard boardCard)
        {
            // Get pudding card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Pudding && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Get count of players
            var playerCount = _unitOfWork.GameRepository.Get(
                    filter: x => x.Id == boardCard.GameId,
                    transform: x => x.AsNoTracking()
                ).First().PlayerIds.Count;

            // Group cards by board and evaluate points
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(c => new { c.Key, Value = c.Count() });

            // Get top points
            var maxCount = boards.MaxBy(b => b.Value)?.Value;
            // Find player entities with the top points
            var maxBoards = boards.Where(b => b.Value == maxCount).Select(b => b.Key).ToList();
            var maxPlayers = _unitOfWork.PlayerRepository.Get(
                        filter: x => maxBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();
            // Add points for each top earner
            foreach (var player in maxPlayers)
            {
                player.Points += 6;
                _unitOfWork.PlayerRepository.Update(player);
            }

            // Calculate last players if there is more than 2 players
            if (playerCount > 2)
            {
                // Get bottom points
                var minCount = boards.MinBy(b => b.Value)?.Value;
                // Find player entities with the bottom points
                var minBoards = boards.Where(b => b.Value == minCount).Select(b => b.Key).ToList();
                var minPlayers = _unitOfWork.PlayerRepository.Get(
                            filter: x => minBoards.Contains(x.BoardId),
                            transform: x => x.AsNoTracking()
                            ).ToList();
                // Add points for each bottom earner
                foreach (var player in minPlayers)
                {
                    player.Points -= 6;
                    _unitOfWork.PlayerRepository.Update(player);
                }
            }
            
            // Set calculated flag for every pudding card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
