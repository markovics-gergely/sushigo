using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class EndGameCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public GameEndDTO GameEndDTO { get; }
        public string CacheKey => $"user-{GameEndDTO.UserId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public EndGameCommand(GameEndDTO gameEnd)
        {
            GameEndDTO = gameEnd;
        }
    }
}
