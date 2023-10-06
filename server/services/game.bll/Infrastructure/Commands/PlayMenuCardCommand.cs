using game.bll.Infrastructure.DataTransferObjects;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class PlayMenuCardCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public PlayCardDTO PlayCardDTO { get; }
        public PlayMenuCardCommand(PlayCardDTO playCardDTO, ClaimsPrincipal? user = null)
        {
            PlayCardDTO = playCardDTO;
            User = user;
        }
    }
}
