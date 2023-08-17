using game.dal.Types;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayCardDTO
    {
        public Guid HandCardId { get; set; }
        public Dictionary<Additional, string> AdditionalInfo { get; set; } = new Dictionary<Additional, string>();
    }
}
