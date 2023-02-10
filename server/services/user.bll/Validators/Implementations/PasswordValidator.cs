using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Validators.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class PasswordValidator : IValidator
    {
        private readonly RegisterUserDTO _dto;

        public PasswordValidator(RegisterUserDTO dto) {
            _dto = dto;
        }

        public bool Validate()
        {
            return _dto.Password != null && _dto.Password == _dto.ConfirmedPassword;
        }
    }
}
