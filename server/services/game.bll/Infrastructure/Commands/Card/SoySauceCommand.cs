using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SoySauceCommand : ICardCommand<SoySauce>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public SoySauceCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get card entities of the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Get board with the most kind of cards
            var groupped = cards
                .GroupBy(c => c.BoardId)
                .MaxBy(c => c
                    .Select(cc => cc.CardType).Distinct().Count() 
                    - Math.Max(0, c.Where(cc => cc.CardType.SushiType() == SushiType.Nigiri).Count() - 1)
                );

            // Get the soy sauce cards of that board
            var soyCards = groupped!.Where(c => c.CardType == CardType.SoySauce && !c.IsCalculated);

            // If there is any soy sauce in that board
            if (soyCards.Any())
            {
                // Get player entity of the winner board
                var player = _unitOfWork.PlayerRepository.Get(
                            filter: x => x.BoardId == groupped!.Key,
                            transform: x => x.AsNoTracking()
                            ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(TeaCommand));
                if (player == null) throw new EntityNotFoundException(nameof(player));

                // Add points for each soy card
                player.Points += 4 * soyCards.Count();
                _unitOfWork.PlayerRepository.Update(player);
            }

            // Set calculated flag for every soy card in the game
            foreach (var card in cards.Where(c => c.CardType == CardType.SoySauce))
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
