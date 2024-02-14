using System.Security.Claims;
using shared.dal.Models.Types;

namespace shared.dal.Models
{
    public class DeckBoughtDTO
    {
        public required Guid UserId { get; init; }
        public required DeckType DeckType { get; init; }
    }
}
