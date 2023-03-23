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
