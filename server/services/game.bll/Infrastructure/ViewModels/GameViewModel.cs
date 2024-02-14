using game.dal.Types;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required DeckType DeckType { get; set; }
        public Guid ActualPlayerId { get; set; }
        public Guid FirstPlayerId { get; set; }
        public int Round { get; set; } = 0;
        public Phase Phase { get; set; } = Phase.None;
        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
