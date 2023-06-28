using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class ClaimPartyCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public PartyBoughtDTO PartyBoughtDTO { get; }
        public string CacheKey => $"user-{PartyBoughtDTO.UserId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public ClaimPartyCommand(PartyBoughtDTO partyBoughtDTO)
        {
            PartyBoughtDTO = partyBoughtDTO;
        }
    }
}
