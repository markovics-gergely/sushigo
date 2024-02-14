using shared.dal.Models.Types;

namespace shop.dal.Domain
{
    public class DeckCard
    {
        public Guid Id { get; set; }
        public CardType CardType { get; set; }
        public Card Card { get; set; } = null!;
        public DeckType DeckType { get; set; }
        public Deck Deck { get; set; } = null!;
    }
}
