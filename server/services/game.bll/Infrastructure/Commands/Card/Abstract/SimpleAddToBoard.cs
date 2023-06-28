using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public class SimpleAddToBoard : ISimpleAddToBoard
    {
        public async Task AddToBoard(IUnitOfWork unitOfWork, Guid handCardId, Guid boardId, ClaimsPrincipal? user = null)
        {
            if (user == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var handCard = unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == handCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddToBoard));
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            unitOfWork.HandCardRepository.Delete(handCardId);
            var boardCard = new BoardCard {
                CardType = handCard.CardType,
                BoardId = boardId,
                GameId = user.GetGameIdFromJwt()
            };
            unitOfWork.BoardCardRepository.Insert(boardCard);
            await unitOfWork.Save();
        }
    }
}
