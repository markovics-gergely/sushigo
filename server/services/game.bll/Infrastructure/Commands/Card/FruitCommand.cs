using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class FruitCommand : ICardCommand<Fruit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;

        public ClaimsPrincipal? User { get; set; }

        public FruitCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
            _addPointByDelegate = addPointByDelegate;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Get fruit card entities in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Fruit && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;

            // Set calculated flag for every fruit card
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

        /// <summary>
        /// Point calculation function of the fruit card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => new[]
            {
                cards.Select(c => int.Parse(c.AdditionalInfo[Additional.Points]) / 100).Count(),
                cards.Select(c => int.Parse(c.AdditionalInfo[Additional.Points]) % 100 / 10).Count(),
                cards.Select(c => int.Parse(c.AdditionalInfo[Additional.Points]) % 10).Count(),
            }.Select(c => c switch
            {
                0 => -2,
                1 => 0,
                2 => 1,
                3 => 3,
                4 => 6,
                _ => 10
            }).Sum();

        public async Task OnEndGame(BoardCard boardCard)
        {
            await _addPointByDelegate.CalculateEndRound(boardCard, CalculateAddPoint);
        }
    }
}
