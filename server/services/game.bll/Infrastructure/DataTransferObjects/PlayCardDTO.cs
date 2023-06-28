namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayCardDTO
    {
        public Guid HandCardId { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
