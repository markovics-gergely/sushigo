using shared.dal.Models;

namespace lobby.dal.Domain
{
    public class Lobby
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime Created { get; set; }
        public DeckType DeckType { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
