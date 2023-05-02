using lobby.bll.Infrastructure.Commands.Cache;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.Models;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class UpdateLobbyDeckCommand : IRequest<LobbyViewModel>, ICacheableMediatrCommandResponse
    {
        public ClaimsPrincipal? User { get; private set; }
        public UpdateLobbyDTO UpdateLobbyDTO { get; private set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{UpdateLobbyDTO.LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public UpdateLobbyDeckCommand(UpdateLobbyDTO updateLobbyDTO, ClaimsPrincipal? user = null)
        {
            User = user;
            UpdateLobbyDTO = updateLobbyDTO;
        }
    }
}
