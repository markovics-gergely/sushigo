using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class GreenTeaIceCreamCommand : ICardCommand<GreenTeaIceCream>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;
        private readonly INoModification _noModification;


        public GreenTeaIceCreamCommand(ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate, INoModification noModification)
        {
            _simpleAddToBoard = simpleAddToBoard;
            _addPointByDelegate = addPointByDelegate;
            _noModification = noModification;
        }

        public Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            return Task.FromResult(_noModification.OnEndRound(boardCard));
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }

        /// <summary>
        /// Point calculation function of the green tea card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => cards.Count() / 4 * 12;

        public async Task<List<Guid>> OnEndGame(BoardCard boardCard)
        {
            return await _addPointByDelegate.CalculateEndRound(boardCard, CalculateAddPoint);
        }
    }
}
