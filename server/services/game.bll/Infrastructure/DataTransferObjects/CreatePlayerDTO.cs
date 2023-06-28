namespace game.bll.Infrastructure.DataTransferObjects
{
    public class CreatePlayerDTO
    {
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
    }
}
