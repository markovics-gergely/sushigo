namespace lobby.dal.Domain
{
    public class Player
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
        public required Guid LobbyId { get; set; }
        public Lobby Lobby { get; set; } = null!;
    }
}
