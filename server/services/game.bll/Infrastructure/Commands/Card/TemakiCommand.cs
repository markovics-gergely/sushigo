using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TemakiCommand : ICardCommand<Temaki>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public TemakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get temaki card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Temaki && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

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
                player.Points += 4;
                _unitOfWork.PlayerRepository.Update(player);
            }

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
                player.Points -= 4;
                _unitOfWork.PlayerRepository.Update(player);
            }

            // Set calculated flag for every temaki card
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
