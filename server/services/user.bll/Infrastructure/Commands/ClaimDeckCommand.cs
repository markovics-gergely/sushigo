using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimDeckCommand : IRequest<UserViewModel>
    {
        public DeckBoughtDTO DeckBoughtDTO { get; }

        public ClaimDeckCommand(DeckBoughtDTO deckBoughtDTO)
        {
            DeckBoughtDTO = deckBoughtDTO;
        }
    }
}
