namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class PlayerDTO
    {
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
        public required Guid LobbyId { get; set; }
    }
}
