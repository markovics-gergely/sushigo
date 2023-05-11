using shared.Models;

namespace lobby.bll.Infrastructure.ViewModels
{
    public class LobbyViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public Guid CreatorUserId { get; set; }
        public required string CreatorUserName { get; set; }
        public DateTime Created { get; set; }
        public DeckType DeckType { get; set; }
        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
