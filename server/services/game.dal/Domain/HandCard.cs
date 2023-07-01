using shared.dal.Models;

namespace game.dal.Domain
{
    public class HandCard
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid HandId { get; set; }
        public Hand? Hand { get; set; }
        public CardType CardType { get; set; }
        public bool IsSelected { get; set; } = false;
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
