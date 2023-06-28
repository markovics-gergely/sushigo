using shared.dal.Models;

namespace game.bll.Infrastructure.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required DeckType DeckType { get; set; }
        public Guid DeckId { get; set; }
        public Guid ActualPlayerId { get; set; }
        public Guid FirstPlayerId { get; set; }
        public int Round { get; set; } = 0;
        public ICollection<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
