namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class JoinLobbyDTO
    {
        public required Guid Id { get; set; }
        public required string Password { get; set; }
    }
}
