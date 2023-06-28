using shared.dal.Models;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class CreateGameDTO
    {
        public required string Name { get; set; }
        public required DeckType DeckType { get; set; }
        public IEnumerable<CreatePlayerDTO> Players { get; set; } = Enumerable.Empty<CreatePlayerDTO>();
    }
}
