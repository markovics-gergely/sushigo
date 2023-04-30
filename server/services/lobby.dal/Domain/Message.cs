namespace lobby.dal.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public required string Text { get; set; }
        public required Guid LobbyId { get; set; }
        public Lobby Lobby { get; set; } = null!;
        public DateTime DateTime { get; set; }
    }
}
