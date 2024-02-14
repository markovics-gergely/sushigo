using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class OnigiriCommand : ICardCommand<Onigiri>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;
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
            .Select(c => c.CardInfo.Point)
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

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            return await _addPointByDelegate.CalculateEndRound(boardCard, CalculateAddPoint);
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }
    }
}
