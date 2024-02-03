using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using shop.bll.Infrastructure.ViewModels;
using System.Security.Claims;

namespace shop.bll.Infrastructure.Queries
{
    public class GetDecksQuery : IRequest<IEnumerable<DeckViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; set; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => "decks";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetDecksQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
