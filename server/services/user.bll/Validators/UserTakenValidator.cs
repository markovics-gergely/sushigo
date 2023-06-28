using Microsoft.EntityFrameworkCore;
using shared.bll.Validators.Interfaces;
using user.bll.Infrastructure.DataTransferObjects;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators
{
    public class UserTakenValidator : IValidator
    {
        private readonly RegisterUserDTO _dto;
        private readonly IUnitOfWork _unitOfWork;

        public UserTakenValidator(RegisterUserDTO dto, IUnitOfWork unitOfWork)
        {
            _dto = dto;
            _unitOfWork = unitOfWork;
        }

        public bool Validate()
        {
            return !_unitOfWork.UserRepository.Get(
                    filter: x => x.Email == _dto.Email || x.UserName == _dto.UserName,
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
