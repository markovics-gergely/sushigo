namespace lobby.dal.Domain
{
    public class Lobby
    {
        public Guid Id { get; set; }
        public required string Password { get; set; }
        public required Guid CreatorId { get; set; }
        public required DateTime Created { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
