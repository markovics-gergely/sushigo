namespace game.bll.Infrastructure.ViewModels
{
    public class PlayerViewModel
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public string? ImagePath { get; set; }
        public int Points { get; set; } = 0;
    }
}
