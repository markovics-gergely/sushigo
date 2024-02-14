using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TeaCommand : ICardCommand<Tea>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public TeaCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get card entities of the player
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == boardCard.BoardId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Nigiri cards count as the same card, so they are calculated separately
            var nigiriCount = cards.Where(c => c.CardInfo.CardType.SushiType() == SushiType.Nigiri).Count();

            // Get card counts by their type and get the one with the top count
            var sushiCount = cards
                .GroupBy(c => c.CardInfo.CardType)
                .Select(c => c.Count())
                .OrderByDescending(c => c)
                .FirstOrDefault();

            // Get the bigger one from the 2 calculations
            var point = new int[] { sushiCount, nigiriCount }.Max();

            // Get player entity of the board
            var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == boardCard.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // Filter to tea cards
            var teaCards = cards.Where(c => c.CardInfo.CardType == CardType.Tea);

            // Add the points to the player for each tea card
            player.Points += point * teaCards.Count();

            _unitOfWork.PlayerRepository.Update(player);
            await _unitOfWork.Save();

            return teaCards.Select(c => c.Id).ToList();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
