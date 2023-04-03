using System.Security.Claims;

namespace shared.Models
{
    public class PartyBoughtDTO
    {
        public required Guid UserId { get; init; }
    }
}
