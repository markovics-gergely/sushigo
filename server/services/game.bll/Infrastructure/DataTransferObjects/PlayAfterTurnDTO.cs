using shared.dal.Models.Types;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayAfterTurnDTO
    {
        public bool IsHandCard { get; set; }
        public Guid HandOrBoardCardId { get; set; }
        public CardType? CardType { get; set; }
        public Guid? TargetCardId { get; set; }
    }
}
