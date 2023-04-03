using shared.Models;

namespace shop.dal.Domain
{
    public class DeckCard
    {
        public CardType CardType { get; set; }
        public Card Card { get; set; } = null!;
        public DeckType DeckType { get; set; }
        public Deck Deck { get; set; } = null!;
    }
}
