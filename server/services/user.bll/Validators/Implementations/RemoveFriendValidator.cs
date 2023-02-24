using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using user.bll.Extensions;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Validators.Interfaces;
using user.dal.Domain;
using user.dal.Types;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class RemoveFriendValidator : IValidator
    {
        private readonly Guid _userId;
        private readonly Guid _friendId;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFriendValidator(Guid userId, Guid friendId, IUnitOfWork unitOfWork)
        {
            _userId = userId;
            _friendId = friendId;
            _unitOfWork = unitOfWork;
        }

        public bool Validate()
        {
            return _unitOfWork.FriendRepository.Get(
                    filter: x => x.OwnFriend(_userId, _friendId),
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
