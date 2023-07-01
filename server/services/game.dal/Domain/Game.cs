using game.dal.Types;
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
        public Phase Phase { get; set; } = Phase.StartGame;

        public ICollection<Player> Players { get; set; } = new List<Player>();
        public List<Guid> PlayerIds { get; set; } = new List<Guid>();
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
