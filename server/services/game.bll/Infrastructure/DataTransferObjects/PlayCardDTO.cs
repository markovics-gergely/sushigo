using game.bll.Infrastructure.ViewModels;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class PlayCardDTO
    {
        public Guid HandCardId { get; set; }
        public CardInfoViewModel CardInfo { get; set; } = new CardInfoViewModel();
    }
}
