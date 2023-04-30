using lobby.bll.Infrastructure.Commands.Cache;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.Domain;
using MediatR;

namespace lobby.bll.Infrastructure.Commands
{
    public class AddPlayerCommand : IRequest<LobbyViewModel>, ICacheableMediatrCommandResponse
    {
        public PlayerDTO PlayerDTO { get; init; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{PlayerDTO.LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public AddPlayerCommand(PlayerDTO playerDTO) {
            PlayerDTO = playerDTO;
        }
    }
}
