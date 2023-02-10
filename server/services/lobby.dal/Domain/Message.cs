namespace lobby.dal.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Text { get; set; }
        public required Guid LobbyId { get; set; }
        public required Lobby Lobby { get; set; }
        public required DateTime DateTime { get; set; }
    }
}
