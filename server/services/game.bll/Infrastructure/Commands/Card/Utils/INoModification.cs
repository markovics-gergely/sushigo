using game.dal.Domain;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public interface INoModification
    {
        public List<Guid> OnEndRound(BoardCard boardCard);
    }
}
