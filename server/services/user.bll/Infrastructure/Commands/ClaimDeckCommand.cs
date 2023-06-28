using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimDeckCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public DeckBoughtDTO DeckBoughtDTO { get; }
        public string CacheKey => $"user-{DeckBoughtDTO.UserId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public ClaimDeckCommand(DeckBoughtDTO deckBoughtDTO)
        {
            DeckBoughtDTO = deckBoughtDTO;
        }
    }
}
