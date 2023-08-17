using game.dal.Domain;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public interface ISimpleAddPoint
    {
        public Task CalculateEndRound(BoardCard boardCard, int point);
    }
}
