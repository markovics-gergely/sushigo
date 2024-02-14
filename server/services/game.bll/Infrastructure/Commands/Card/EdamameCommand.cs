using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class EdamameCommand : ICardCommand<Edamame>
    {
        private static readonly int MAX_POINT = 4;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public EdamameCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get edamame card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                ).Where(x => x.CardInfo.CardType == CardType.Edamame);
            if (!cards.Any()) return new() { boardCard.Id };

            // Goup cards by the boards of the players
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(c => new { BoardId = c.Key, Count = c.Count() });

            // If there is more than 1 player who played edamame
            if (boards.Count() > 1)
            {
                foreach (var board in boards)
                {
                    // Get player entity of the board
                    var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == board.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

                    // Add point for each other player who played
                    player.Points += board.Count * Math.Min(boards.Count() - 1, MAX_POINT);
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
