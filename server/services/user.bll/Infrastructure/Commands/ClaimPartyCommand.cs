using MediatR;
using shared.Models;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimPartyCommand : IRequest
    {
        public PartyBoughtDTO PartyBoughtDTO { get; }

        public ClaimPartyCommand(PartyBoughtDTO partyBoughtDTO)
        {
            PartyBoughtDTO = partyBoughtDTO;
        }
    }
}
