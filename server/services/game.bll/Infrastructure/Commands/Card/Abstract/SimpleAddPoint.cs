using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public abstract class SimpleAddPoint : ISimpleAddPoint
    {
        public async Task CalculateEndRound(IUnitOfWork unitOfWork, BoardCard boardCard, int point, ClaimsPrincipal? user = null)
        {
            if (user == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var player = unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == user.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            if (player == null) return;
            player.Points += point;
            unitOfWork.PlayerRepository.Update(player);
            boardCard.IsCalculated = true;
            unitOfWork.BoardCardRepository.Update(boardCard);
            await unitOfWork.Save();
        }
    }
}
