using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace game.bll.Infrastructure.Commands
{
    public class CalculateEndRoundCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public CalculateEndRoundCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
