using game.dal.Types;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayAfterTurnDTO
    {
        public Guid? BoardCardId { get; set; }
        public Guid? HandCardId { get; set; }
        public Dictionary<Additional, string> AdditionalInfo { get; set; } = new Dictionary<Additional, string>();
    }
}
