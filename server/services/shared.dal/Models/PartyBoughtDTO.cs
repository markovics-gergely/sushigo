using System.Security.Claims;

namespace shared.dal.Models
{
    public class PartyBoughtDTO
    {
        public required Guid UserId { get; init; }
    }
}
