using game.dal.Domain;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public delegate int CalculatePoint(IEnumerable<BoardCard> cards);

    /// <summary>
    /// Helper class for cards with simple point calculation
    /// </summary>
    public interface IAddPointByDelegate
    {
        public Task CalculateEndRound(BoardCard boardCard, CalculatePoint calculatePoint);
    }
}
