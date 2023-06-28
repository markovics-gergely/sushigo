using shared.dal.Models;

namespace game.dal.Domain
{
    public class Deck
    {
        public Guid Id { get; set; }
        public Game Game { get; set; } = null!;
        public ICollection<CardType> Cards { get; set; } = new List<CardType>();
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
