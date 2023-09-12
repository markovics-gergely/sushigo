using shared.dal.Models;

namespace shop.bll.Infrastructure.ViewModels
{
    public class DeckItemViewModel
    {
        public DeckType DeckType { get; set; }
        public required string ImagePath { get; set; }
        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }
        public IEnumerable<CardType> CardTypes { get; set; } = Enumerable.Empty<CardType>();
    }
}
