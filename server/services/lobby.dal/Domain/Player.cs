namespace lobby.dal.Domain
{
    public class Player
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid LobbyId { get; set; }
        public required Lobby Lobby { get; set; }
    }
}
