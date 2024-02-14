using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    /// <summary>
    /// Command class to execute dumpling card functions
    /// </summary>
    public class DumplingCommand : ICardCommand<Dumpling>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;

        public DumplingCommand(ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate)
        {
            _simpleAddToBoard = simpleAddToBoard;
            _addPointByDelegate = addPointByDelegate;
        }

        /// <summary>
        /// Point calculation function of the dumpling card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => cards.Count() switch
            {
                0 => 0,
                1 => 1,
                2 => 3,
                3 => 6,
                4 => 10,
                _ => 15
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
