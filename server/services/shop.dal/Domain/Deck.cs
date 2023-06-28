using shared.dal.Models;

namespace shop.dal.Domain
{
    public class Deck
    {
        public Guid Id { get; set; }
        public DeckType DeckType { get; set; }
        public long Cost { get; set; }
        public required string ImagePath { get; set; }
        public ICollection<DeckCard> Cards { get; set; } = new HashSet<DeckCard>();
    }
}
