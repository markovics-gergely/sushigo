using game.dal.Domain;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public delegate int CalculatePoint(IEnumerable<BoardCard> cards);

    /// <summary>
    /// Helper class for cards with simple point calculation
    /// </summary>
    public interface IAddPointByDelegate
    {
        /// <summary>
        /// Calculate the points of the cards at the end of the round
        /// </summary>
        /// <param name="boardCard"></param>
        /// <param name="calculatePoint"></param>
        /// <returns>List of the ids of the cards that were calculated</returns>
        public Task<List<Guid>> CalculateEndRound(BoardCard boardCard, CalculatePoint calculatePoint);
    }
}
