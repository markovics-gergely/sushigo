namespace game.dal.Domain
{
    public class Hand
    {
        public Guid Id { get; set; }
        public Player? Player { get; set; }
        public ICollection<HandCard> Cards { get; set; } = new List<HandCard>();
    }
}
