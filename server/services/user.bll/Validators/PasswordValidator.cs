using shared.bll.Validators.Interfaces;
using user.bll.Infrastructure.DataTransferObjects;

namespace user.bll.Validators
{
    public class PasswordValidator : IValidator
    {
        private readonly RegisterUserDTO _dto;

        public PasswordValidator(RegisterUserDTO dto)
        {
            _dto = dto;
        }

        public bool Validate()
        {
            return _dto.Password != null && _dto.Password == _dto.ConfirmedPassword;
        }
    }
}
