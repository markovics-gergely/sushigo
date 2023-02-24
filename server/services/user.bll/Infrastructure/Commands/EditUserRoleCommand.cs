using MediatR;
using user.bll.Infrastructure.DataTransferObjects;

namespace user.bll.Infrastructure.Commands
{
    public class EditUserRoleCommand : IRequest
    {
        public EditUserRoleDTO DTO { get; }

        public EditUserRoleCommand(EditUserRoleDTO dto)
        {
            DTO = dto;
        }
    }
}
