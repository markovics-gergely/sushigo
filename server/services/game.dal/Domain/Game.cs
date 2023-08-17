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
        public Phase Phase { get; set; } = Phase.Turn;

        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Guid> PlayerIds { get; set; } = new List<Guid>();
        public Dictionary<CardType, string> AdditionalInfo { get; set; } = new Dictionary<CardType, string>();

        public bool IsOver() { return Round >= 2; }
        public static int GetHandCount(int playerCount)
        {
            if (playerCount < 4) return 10;
            if (playerCount < 6) return 9;
            if (playerCount < 8) return 8;
            return 7;
        }
        public int GetHandCount()
        {
            return GetHandCount(PlayerIds.Count);
        }
    }
}
