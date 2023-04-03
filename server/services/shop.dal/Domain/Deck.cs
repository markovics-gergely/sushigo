using shared.Models;

namespace shop.dal.Domain
{
    public class Deck
    {
        public Guid Id { get; set; }
        public DeckType DeckType { get; set; }
        public long Cost { get; set; }
        public ICollection<DeckCard> Cards { get; set; } = new HashSet<DeckCard>();

    }
}
