using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace game.bll.Infrastructure.Commands.Card.Utils.Implementations
{
    public class NoModification : INoModification
    {
        private readonly IUnitOfWork _unitOfWork;


        public NoModification(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Guid> OnEndRound(BoardCard boardCard)
        {
            // Get card entities with the same type in the game
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardInfo.CardType == boardCard.CardInfo.CardType,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                );
            return cards.Select(x => x.Id).ToList();
        }
    }
}
