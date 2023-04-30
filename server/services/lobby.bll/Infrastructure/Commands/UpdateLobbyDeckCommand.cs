using lobby.bll.Infrastructure.Commands.Cache;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.Models;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class UpdateLobbyDeckCommand : IRequest<LobbyViewModel>, ICacheableMediatrCommandResponse
    {
        public ClaimsPrincipal? User { get; private set; }
        public Guid LobbyId { get; private set; }
        public DeckType DeckType { get; private set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public UpdateLobbyDeckCommand(Guid lobbyId, DeckType deckType, ClaimsPrincipal? user = null)
        {
            User = user;
            LobbyId = lobbyId;
            DeckType = deckType;
        }
    }
}
