using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public interface ISimpleAddPoint
    {
        public Task CalculateEndRound(IUnitOfWork unitOfWork, BoardCard boardCard, int point, ClaimsPrincipal? user = null);
    }
}
