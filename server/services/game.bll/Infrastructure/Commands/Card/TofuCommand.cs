using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TofuCommand : ICardCommand<Tofu>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;

        public TofuCommand(ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate)
        {
            _simpleAddToBoard = simpleAddToBoard;
            _addPointByDelegate = addPointByDelegate;
        }

        /// <summary>
        /// Point calculation function of the tofu card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => cards.Count() switch
            {
                1 => 2,
                2 => 6,
                _ => 0
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
