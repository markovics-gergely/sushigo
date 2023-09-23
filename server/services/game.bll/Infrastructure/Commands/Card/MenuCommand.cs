using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class MenuCommand : ICardCommand<Menu>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public ClaimsPrincipal? User { get; set; }

        public MenuCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get menu card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Menu && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Set calculated flag for every menu card
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.GameId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            
        }
    }
}
