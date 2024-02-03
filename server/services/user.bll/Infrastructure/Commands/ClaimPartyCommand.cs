using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimPartyCommand : IRequest<UserViewModel>
    {
        public PartyBoughtDTO PartyBoughtDTO { get; }

        public ClaimPartyCommand(PartyBoughtDTO partyBoughtDTO)
        {
            PartyBoughtDTO = partyBoughtDTO;
        }
    }
}
