namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class RemovePlayerDTO
    {
        public required Guid PlayerId { get; set; }
        public required Guid LobbyId { get; set; }
    }
}
