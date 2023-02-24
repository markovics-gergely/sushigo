using MediatR;
using user.bll.Infrastructure.DataTransferObjects;

namespace user.bll.Infrastructure.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
        public RegisterUserDTO DTO { get; }

        public CreateUserCommand(RegisterUserDTO dto)
        {
            DTO = dto;
        }
    }
}
