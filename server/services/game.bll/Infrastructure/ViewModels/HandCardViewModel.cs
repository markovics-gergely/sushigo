namespace game.bll.Infrastructure.ViewModels
{
    public class HandCardViewModel
    {
        public Guid Id { get; set; }
        public bool IsSelected { get; set; } = false;
        public CardInfoViewModel CardInfo { get; set; } = new CardInfoViewModel();
    }
}
