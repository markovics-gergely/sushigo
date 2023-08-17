using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TeaCommand : ICardCommand<Tea>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public TeaCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get card entities of the player
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == boardCard.BoardId,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Nigiri cards count as the same card, so they are calculated separately
            var nigiriCount = cards.Where(c => c.CardType.SushiType() == SushiType.Nigiri).Count();

            // Get card counts by their type and get the one with the top count
            var sushiCount = cards
                .GroupBy(c => c.CardType)
                .Select(c => c.Count())
                .OrderByDescending(c => c)
                .FirstOrDefault();

            // Get the bigger one from the 2 calculations
            var point = new int[] { sushiCount, nigiriCount }.Max();

            // Get player entity of the board
            var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == boardCard.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(TeaCommand));
            if (player == null) throw new EntityNotFoundException(nameof(player));

            // Filter to tea cards which are not calculated already
            foreach (var card in cards.Where(c => c.CardType == CardType.Tea && !c.IsCalculated))
            {
                // Set calculated flag for the card
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);

                // Add the points to the player for each tea card
                player.Points += point;
            }
            _unitOfWork.PlayerRepository.Update(player);
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
