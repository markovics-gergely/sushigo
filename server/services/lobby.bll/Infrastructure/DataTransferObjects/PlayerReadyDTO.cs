namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class PlayerReadyDTO
    {
        public Guid PlayerId { get; set; }
        public bool Ready { get; set; } = true;
    }
}
