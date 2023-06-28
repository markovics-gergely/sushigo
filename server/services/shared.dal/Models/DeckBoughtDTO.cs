using System.Security.Claims;

namespace shared.dal.Models
{
    public class DeckBoughtDTO
    {
        public required Guid UserId { get; init; }
        public required DeckType DeckType { get; init; }
    }
}
