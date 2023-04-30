namespace lobby.bll.Infrastructure.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Text { get; set; }
        public required DateTime DateTime { get; set; }
    }
}
