using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using user.bll.Exceptions;
using user.bll.Extensions;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;
using user.bll.Validators.Interfaces;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Infrastructure
{
    public class UserQueryHandler :
        IRequestHandler<GetUserQuery, UserViewModel>,
        IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public UserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserNameViewModel>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            var set = new HashSet<UserNameViewModel>();
            foreach (var role in request.Roles)
            {
                var users = _mapper.Map<IEnumerable<UserNameViewModel>>(await _userManager.GetUsersInRoleAsync(role));
                set.UnionWith(users);
            }
            return set.AsEnumerable();
        }

        public Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException("Requested user not found");
            }
            var userEntity = _unitOfWork.UserRepository.Get(
                filter: x => x.Id.ToString() == (request.Id ?? request.User.GetUserIdFromJwt()),
                transform: x => x.AsNoTracking())
                .FirstOrDefault();

            if (userEntity == null)
            {
                throw new EntityNotFoundException("Requested user not found");
            }
            var userViewModel = _mapper.Map<UserViewModel>(userEntity);
            return Task.FromResult(userViewModel);
        }
    }
}
