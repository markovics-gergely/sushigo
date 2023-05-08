namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class PlayerDTO
    {
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required Guid LobbyId { get; set; }
    }
}
