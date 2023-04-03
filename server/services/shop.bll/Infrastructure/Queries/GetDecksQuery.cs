using MediatR;
using shop.bll.Extensions;
using shop.bll.Infrastructure.Queries.Cache;
using shop.bll.Infrastructure.ViewModels;
using System.Security.Claims;

namespace shop.bll.Infrastructure.Queries
{
    public class GetDecksQuery : IRequest<IEnumerable<DeckViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => "decks";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetDecksQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
