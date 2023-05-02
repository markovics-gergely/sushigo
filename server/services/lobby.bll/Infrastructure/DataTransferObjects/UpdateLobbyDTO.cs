using shared.Models;

namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class UpdateLobbyDTO
    {
        public Guid LobbyId { get; private set; }
        public DeckType DeckType { get; private set; }
    }
}
