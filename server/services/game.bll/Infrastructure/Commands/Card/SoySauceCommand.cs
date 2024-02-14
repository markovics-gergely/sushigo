using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SoySauceCommand : ICardCommand<SoySauce>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public SoySauceCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            // Get card entities of the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            if (!cards.Any()) return new() { boardCard.Id };

            // Get board with the most kind of cards
            // Nigiri cards are counted as 1
            var groupped = cards
                .GroupBy(c => c.BoardId)
                .MaxBy(c => c
                    .Select(cc => cc.CardInfo.CardType).Distinct().Count()
                    - Math.Max(0, c.Where(cc => cc.CardInfo.CardType.SushiType() == SushiType.Nigiri).Count() - 1)
                );

            // Get the soy sauce cards of that board
            var soyCards = groupped!.Where(c => c.CardInfo.CardType == CardType.SoySauce);

            // If there is any soy sauce in that board
            if (soyCards.Any())
            {
                // Get player entity of the winner board
                var player = _unitOfWork.PlayerRepository.Get(
                            filter: x => x.BoardId == groupped!.Key,
                            transform: x => x.AsNoTracking()
                            ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(TeaCommand));

                // Add points for each soy card
                player.Points += 4 * soyCards.Count();
                _unitOfWork.PlayerRepository.Update(player);
            }
            await _unitOfWork.Save();

            // Exclude all soy cards from the next calculation
            return cards.Where(x => x.CardInfo.CardType == CardType.SoySauce).Select(x => x.Id).ToList();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
