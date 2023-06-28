using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class JoinLobbyCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public LobbyJoinedDTO LobbyJoinedDTO { get; }
        public string CacheKey => $"user-{LobbyJoinedDTO.UserId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public JoinLobbyCommand(LobbyJoinedDTO lobbyJoinedDTO)
        {
            LobbyJoinedDTO = lobbyJoinedDTO;
        }
    }
}
