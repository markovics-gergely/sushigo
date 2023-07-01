using game.bll.Infrastructure.DataTransferObjects;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class PlayCardCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public PlayCardDTO PlayCardDTO { get; }
        public PlayCardCommand(PlayCardDTO playCardDTO, ClaimsPrincipal? user = null)
        {
            PlayCardDTO = playCardDTO;
            User = user;
        }
    }
}
