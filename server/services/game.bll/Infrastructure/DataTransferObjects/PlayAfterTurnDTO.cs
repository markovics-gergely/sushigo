namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayAfterTurnDTO
    {
        public Guid BoardCardId { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
