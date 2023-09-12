namespace shared.dal.Models
{
    public class GameEndDTO
    {
        public int Points { get; set; }
        public Guid UserId { get; set; }
        public required string GameName { get; set; }
    }
}
