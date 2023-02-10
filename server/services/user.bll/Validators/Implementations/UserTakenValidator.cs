using Microsoft.EntityFrameworkCore;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Validators.Interfaces;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class UserTakenValidator : IValidator
    {
        private readonly RegisterUserDTO _dto;
        private readonly IUnitOfWork _unitOfWork;

        public UserTakenValidator(RegisterUserDTO dto, IUnitOfWork unitOfWork) {
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
