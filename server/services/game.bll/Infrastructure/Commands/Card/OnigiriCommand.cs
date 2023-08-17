using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class OnigiriCommand : ICardCommand<Onigiri>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;
        public ClaimsPrincipal? User { get; set; }

        public OnigiriCommand(ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate)
        {
            _simpleAddToBoard = simpleAddToBoard;
            _addPointByDelegate = addPointByDelegate;
        }

        /// <summary>
        /// Point calculation function of the onigiri card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => cards
            .Select(c => c.AdditionalInfo[Additional.Points])
            .Distinct()
            .Count()
            switch
            {
                0 => 0,
                1 => 1,
                2 => 4,
                3 => 9,
                _ => 16,
            };

        public async Task OnEndRound(BoardCard boardCard)
        {
            await _addPointByDelegate.CalculateEndRound(boardCard, CalculateAddPoint);
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
