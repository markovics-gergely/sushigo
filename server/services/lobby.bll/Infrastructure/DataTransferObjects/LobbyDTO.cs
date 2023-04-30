namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class LobbyDTO
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string CreatorImagePath { get; set; } = "";
    }
}
