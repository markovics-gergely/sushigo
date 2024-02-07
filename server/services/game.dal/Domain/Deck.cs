using shared.dal.Models;

namespace game.dal.Domain
{
    public class Deck
    {
        public Guid Id { get; set; }
        public DeckType DeckType { get; set; }
        public Game Game { get; set; } = null!;
        public Queue<CardTypePoint> Cards { get; set; } = new Queue<CardTypePoint>();
    }
}
