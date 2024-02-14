using game.bll.Infrastructure.Commands.Card.Utils;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class FruitCommand : ICardCommand<Fruit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly IAddPointByDelegate _addPointByDelegate;
        private readonly INoModification _noModification;


        public FruitCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, IAddPointByDelegate addPointByDelegate, INoModification noModification)
        {
            _unitOfWork = unitOfWork;
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
        /// Point calculation function of the fruit card
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static int CalculateAddPoint(IEnumerable<BoardCard> cards) => new[]
            {
                cards.Select(c => c.CardInfo.Point / 100).Count(),
                cards.Select(c => c.CardInfo.Point % 100 / 10).Count(),
                cards.Select(c => c.CardInfo.Point % 10).Count(),
            }.Select(c => c switch
            {
                0 => -2,
                1 => 0,
                2 => 1,
                3 => 3,
                4 => 6,
                _ => 10
            }).Sum();

        public async Task<List<Guid>> OnEndGame(BoardCard boardCard)
        {
            return await _addPointByDelegate.CalculateEndRound(boardCard, CalculateAddPoint);
        }
    }
}
