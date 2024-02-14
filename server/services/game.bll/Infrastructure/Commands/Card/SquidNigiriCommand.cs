using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SquidNigiriCommand : ICardCommand<SquidNigiri>
    {
        private static readonly int POINT = 3;
        private readonly ISimpleAddPoint _simpleAddPoint;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public SquidNigiriCommand(ISimpleAddPoint simpleAddPoint, ISimpleAddToBoard simpleAddToBoard)
        {
            _simpleAddPoint = simpleAddPoint;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            var wasabiUsed = boardCard.CardInfo.CustomTag == CardTagType.WASABI;
            await _simpleAddPoint.CalculateEndRound(boardCard, wasabiUsed ? POINT * 2 : POINT);
            return new() { boardCard.Id };
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddNigiriToBoard(player, handCard);
        }
    }
}
