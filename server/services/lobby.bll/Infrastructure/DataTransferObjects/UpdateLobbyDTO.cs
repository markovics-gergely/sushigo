using shared.dal.Models.Types;

namespace lobby.bll.Infrastructure.DataTransferObjects
{
    public class UpdateLobbyDTO
    {
        public Guid LobbyId { get; set; }
        public DeckType DeckType { get; set; }
    }
}
