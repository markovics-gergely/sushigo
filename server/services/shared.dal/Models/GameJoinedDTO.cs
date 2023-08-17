namespace shared.dal.Models
{
    public class GameJoinedDTO
    {
        public Guid LobbyId { get; set; } = Guid.Empty;
        public Guid? GameId { get; set; }
        public IEnumerable<Guid> UserIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
