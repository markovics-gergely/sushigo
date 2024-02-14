using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class WasabiCommand : ICardCommand<Wasabi>
    {
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly INoModification _noModification;
        public WasabiCommand(ISimpleAddToBoard simpleAddToBoard, INoModification noModification)
        {
            _simpleAddToBoard = simpleAddToBoard;
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
    }
}
