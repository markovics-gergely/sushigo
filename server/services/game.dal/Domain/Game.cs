using shared.dal.Models;

namespace game.dal.Domain
{
    public class Game
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required DeckType DeckType { get; set; }
        public Guid DeckId { get; set; }
        public Deck? Deck { get; set; }
        public Guid ActualPlayerId { get; set; }
        public Guid FirstPlayerId { get; set; }
        public int Round { get; set; } = 0;

        public ICollection<Player> Players { get; set; } = new List<Player>();
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
