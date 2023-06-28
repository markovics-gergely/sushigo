using Microsoft.EntityFrameworkCore;
using shared.bll.Validators.Interfaces;
using user.bll.Infrastructure.DataTransferObjects;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators
{
    public class EditUserValidator : IValidator
    {
        private readonly EditUserDTO _dto;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _id;

        public EditUserValidator(EditUserDTO dto, IUnitOfWork unitOfWork, string id)
        {
            _dto = dto;
            _unitOfWork = unitOfWork;
            _id = id;
        }
        public bool Validate()
        {
            return !_unitOfWork.UserRepository.Get(
                    filter: x => x.UserName == _dto.UserName && x.Id.ToString() != _id,
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
