namespace user.bll.Infrastructure.ViewModels
{
    public class HistoryViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Placement { get; set; }
        public int Point { get; set; }
        public DateTime Created { get; set; }
    }
}
