using MediatR;
using shared.Models;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimDeckCommand : IRequest
    {
        public DeckBoughtDTO DeckBoughtDTO { get; }

        public ClaimDeckCommand(DeckBoughtDTO deckBoughtDTO)
        {
            DeckBoughtDTO = deckBoughtDTO;
        }
    }
}
