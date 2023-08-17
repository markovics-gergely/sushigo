using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class JoinGameCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public GameJoinedSingleDTO GameJoinedSingleDTO { get; }
        public string CacheKey => $"user-{GameJoinedSingleDTO.UserId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public JoinGameCommand(GameJoinedSingleDTO gameJoinedSingleDTO)
        {
            GameJoinedSingleDTO = gameJoinedSingleDTO;
        }
    }
}
