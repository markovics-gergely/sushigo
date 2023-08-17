using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SalmonNigiriCommand : ICardCommand<SalmonNigiri>
    {
        private static readonly int POINT = 2;
        private readonly ISimpleAddPoint _simpleAddPoint;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }
        public SalmonNigiriCommand(ISimpleAddPoint simpleAddPoint, ISimpleAddToBoard simpleAddToBoard)
        {
            _simpleAddPoint = simpleAddPoint;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            var wasabi = boardCard.AdditionalInfo.ContainsKey(dal.Types.Additional.Tagged);
            await _simpleAddPoint.CalculateEndRound(boardCard, wasabi ? POINT * 2 : POINT);
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddNigiriToBoard(player, handCard);
        }
    }
}
