using MediatR;
using shared.dal.Models.Types;
using shop.bll.Infrastructure.ViewModels;

namespace shop.bll.Infrastructure.Queries
{
    public class GetDeckQuery : IRequest<DeckItemViewModel>
    {
        public DeckType DeckType { get; init; }

        public GetDeckQuery(DeckType deckType)
        {
            DeckType = deckType;
        }
    }
}
