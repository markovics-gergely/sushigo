namespace game.dal.Domain
{
    public class Player
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
        public int Points { get; set; } = 0;
        public bool CanPlayAfterTurn { get; set; } = false;
        public Guid GameId { get; set; }
        public Game? Game { get; set; }
        public Guid HandId { get; set; }
        public Hand? Hand { get; set; }
        public Guid BoardId { get; set; }
        public Board? Board { get; set; }
        public Guid NextPlayerId { get; set; }
    }
}
