namespace game.bll.Infrastructure.ViewModels
{
    public class BoardViewModel
    {
        public IEnumerable<BoardCardViewModel> Cards { get; set; } = new List<BoardCardViewModel>();
    }
}
