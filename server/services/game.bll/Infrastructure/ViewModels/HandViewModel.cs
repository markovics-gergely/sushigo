namespace game.bll.Infrastructure.ViewModels
{
    public class HandViewModel
    {
        public IEnumerable<HandCardViewModel> Cards { get; set; } = new List<HandCardViewModel>();
    }
}
