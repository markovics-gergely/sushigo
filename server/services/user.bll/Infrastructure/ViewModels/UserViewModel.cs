namespace user.bll.Infrastructure.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public long Experience { get; set; }
        public long Coin { get; set; }
    }
}
