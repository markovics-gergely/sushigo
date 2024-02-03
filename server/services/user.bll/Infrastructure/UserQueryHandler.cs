using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Infrastructure
{
    public class UserQueryHandler :
        IRequestHandler<GetUserQuery, UserViewModel>,
        IRequestHandler<GetUserByIdQuery, UserViewModel>,
        IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>,
        IRequestHandler<GetHistoryQuery, IEnumerable<HistoryViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

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
                includeProperties: nameof(ApplicationUser.Avatar),
                transform: x => x.AsNoTracking())
                .FirstOrDefault();

            if (userEntity == null)
            {
                throw new EntityNotFoundException("Requested user not found");
            }
            var userViewModel = _mapper.Map<UserViewModel>(userEntity);
            return Task.FromResult(userViewModel);
        }

        public Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userEntity = _unitOfWork.UserRepository.Get(
                filter: x => x.Id.ToString() == request.Id,
                includeProperties: nameof(ApplicationUser.Avatar),
                transform: x => x.AsNoTracking())
                .FirstOrDefault();

            if (userEntity == null)
            {
                throw new EntityNotFoundException("Requested user not found");
            }
            var userViewModel = _mapper.Map<UserViewModel>(userEntity);
            return Task.FromResult(userViewModel);
        }

        public Task<IEnumerable<HistoryViewModel>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = request.User?.GetUserIdFromJwt() ?? "";
            var historyEntity = _unitOfWork.HistoryRepository.Get(
                filter: x => x.UserId.ToString() == userId,
                transform: x => x.AsNoTracking())
                .ToList();
            var historyViewModel = _mapper.Map<IEnumerable<HistoryViewModel>>(historyEntity);
            return Task.FromResult(historyViewModel);
        }
    }
}
