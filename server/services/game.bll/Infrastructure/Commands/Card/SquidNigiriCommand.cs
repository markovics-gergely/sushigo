using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SquidNigiriCommand : ICardCommand<SquidNigiri>
    {
        private static readonly int POINT = 3;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddPoint _simpleAddPoint;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }
        public SquidNigiriCommand(IUnitOfWork unitOfWork, ISimpleAddPoint simpleAddPoint, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddPoint = simpleAddPoint;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task CalculateEndRound(BoardCard boardCard)
        {
            await _simpleAddPoint.CalculateEndRound(_unitOfWork, boardCard, POINT, User);
        }

        public async Task OnPlay(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }
    }
}
