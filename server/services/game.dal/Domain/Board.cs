namespace game.dal.Domain
{
    public class Board
    {
        public Guid Id { get; set; }
        public Player? Player { get; set; }
        public Guid GameId { get; set; }
        public ICollection<BoardCard> Cards { get; set; } = new List<BoardCard>();
    }
}
