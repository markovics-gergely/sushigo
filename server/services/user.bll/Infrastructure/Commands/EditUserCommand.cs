using MediatR;
using System.Security.Claims;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class EditUserCommand : IRequest<UserViewModel>
    {
        public EditUserDTO DTO { get; }
        public ClaimsPrincipal? User { get; }

        public EditUserCommand(EditUserDTO dto, ClaimsPrincipal? user = null)
        {
            DTO = dto;
            User = user;
        }
    }
}
