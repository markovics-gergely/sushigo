using game.bll.Infrastructure.DataTransferObjects;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class PlayAfterTurnCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public PlayAfterTurnDTO PlayAfterTurnDTO { get; }
        public PlayAfterTurnCommand(PlayAfterTurnDTO playAfterTurnDTO, ClaimsPrincipal? user = null)
        {
            PlayAfterTurnDTO = playAfterTurnDTO;
            User = user;
        }
    }
}
