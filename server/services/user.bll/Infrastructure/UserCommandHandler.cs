using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using user.bll.Infrastructure.Commands;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;
using user.bll.Validators.Interfaces;
using user.bll.Validators.Implementations;
using user.bll.Exceptions;
using user.bll.Extensions;

namespace user.bll.Infrastructure
{
    public class UserCommandHandler :
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<EditUserCommand, bool>,
        IRequestHandler<ClaimPartyCommand, bool>,
        IRequestHandler<EditUserRoleCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _validator = new EmailValidator(request.DTO.Email);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Invalid e-mail format");
            }
            _validator = new PasswordValidator(request.DTO);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Invalid password confirmation");
            }
            _validator = new UserTakenValidator(request.DTO, _unitOfWork);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Username or e-mail is already taken");
            }

            var user = _mapper.Map<ApplicationUser>(request.DTO);
            var result = await _userManager.CreateAsync(user, request.DTO.Password);
            if (result.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
            }
            return result.Succeeded;
        }

        public async Task<bool> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id.ToString() == request.User.GetUserIdFromJwt()).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new EditUserValidator(request.DTO, _unitOfWork);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Invalid modification");
            }
            userEntity.UserName = request.DTO.UserName;
            userEntity.FirstName = request.DTO.FirstName;
            userEntity.LastName = request.DTO.LastName;

            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<Unit> Handle(EditUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id == request.DTO.Id).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            //var roles = await _userManager.GetRolesAsync(userEntity);
            //var removed = await _userManager.RemoveFromRolesAsync(userEntity, roles);
            //if (removed.Errors.Any())
            //{
            //    throw new InvalidParameterException(string.Join('\n', removed.Errors.Select(e => e.Description).ToList()));
            //}
            var added = await _userManager.AddToRoleAsync(userEntity, request.DTO.Role);
            if (added.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', added.Errors.Select(e => e.Description).ToList()));
            }
            return Unit.Value;
        }

        public Task<bool> Handle(ClaimPartyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
