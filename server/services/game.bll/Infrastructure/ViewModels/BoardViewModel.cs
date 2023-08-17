namespace game.bll.Infrastructure.ViewModels
{
    public class BoardViewModel
    {
        IEnumerable<BoardCardViewModel> Cards { get; set; } = new List<BoardCardViewModel>();
    }
}
