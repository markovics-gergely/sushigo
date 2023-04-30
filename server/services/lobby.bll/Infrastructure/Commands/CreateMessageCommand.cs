using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class CreateMessageCommand : IRequest<MessageViewModel>
    {
        public ClaimsPrincipal? User { get; init; }
        public MessageDTO Message { get; init; }
        public CreateMessageCommand(MessageDTO message, ClaimsPrincipal? user = null)
        {
            Message = message;
            User = user;
        }
    }
}
