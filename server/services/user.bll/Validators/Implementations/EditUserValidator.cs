using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Validators.Interfaces;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class EditUserValidator : IValidator
    {
        private readonly EditUserDTO _dto;
        private readonly IUnitOfWork _unitOfWork;

        public EditUserValidator(EditUserDTO dto, IUnitOfWork unitOfWork)
        {
            _dto = dto;
            _unitOfWork = unitOfWork;
        }
        public bool Validate()
        {
            return !_unitOfWork.UserRepository.Get(
                    filter: x => x.UserName == _dto.UserName,
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
