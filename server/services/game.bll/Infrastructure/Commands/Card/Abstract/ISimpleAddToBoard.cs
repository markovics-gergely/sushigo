using game.dal.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public interface ISimpleAddToBoard
    {
        public Task AddToBoard(IUnitOfWork unitOfWork, Guid handCardId, Guid boardId, ClaimsPrincipal? user = null);
    }
}
