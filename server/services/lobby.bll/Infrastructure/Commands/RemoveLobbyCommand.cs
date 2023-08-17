using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Infrastructure.Queries;

namespace lobby.bll.Infrastructure.Commands
{
    public class RemoveLobbyCommand : IRequest<LobbyViewModel?>, ICacheableMediatrCommandResponse
    {
        public Guid LobbyId { get; init; } = Guid.Empty;
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }
        public RemoveLobbyCommand(Guid lobbyId) {
            LobbyId = lobbyId;
        }
    }
}
