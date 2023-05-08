using lobby.bll.Infrastructure.Commands.Cache;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace lobby.bll.Infrastructure.Commands
{
    public class JoinLobbyCommand : IRequest<LobbyViewModel>, ICacheableMediatrCommandResponse
    {
        public ClaimsPrincipal? User { get; init; }
        public JoinLobbyDTO JoinLobbyDTO { get; init; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{JoinLobbyDTO.Id}";
        public TimeSpan? SlidingExpiration { get; set; }

        public JoinLobbyCommand(JoinLobbyDTO joinLobbyDTO, ClaimsPrincipal? user = null)
        {
            JoinLobbyDTO = joinLobbyDTO;
            User = user;
        }
    }
}
