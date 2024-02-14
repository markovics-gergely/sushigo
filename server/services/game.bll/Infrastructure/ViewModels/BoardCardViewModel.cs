namespace game.bll.Infrastructure.ViewModels
{
    public class BoardCardViewModel
    {
        public Guid Id { get; set; }
        public CardInfoViewModel CardInfo { get; set; } = new CardInfoViewModel();
    }
}
