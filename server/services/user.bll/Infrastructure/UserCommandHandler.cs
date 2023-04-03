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
using user.dal.Configurations.Interfaces;
using user.dal.Repository.Interfaces;
using user.bll.Infrastructure.ViewModels;
using shared.Models;

namespace user.bll.Infrastructure
{
    public class UserCommandHandler :
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<EditUserCommand, UserViewModel>,
        IRequestHandler<ClaimPartyCommand>,
        IRequestHandler<ClaimDeckCommand>,
        IRequestHandler<EditUserRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserConfigurationService _config;
        private readonly IFileRepository _fileRepository;

        public UserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager, IUserConfigurationService config, IFileRepository fileRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _fileRepository = fileRepository;
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
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, RoleTypes.Classic);
                if (roleResult.Errors.Any())
                {
                    throw new InvalidParameterException(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
                }
            }
            return result.Succeeded;
        }

        public async Task<UserViewModel> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var userEntity = _unitOfWork.UserRepository.Get(
                filter: x => x.Id.ToString() == request.User.GetUserIdFromJwt(),
                includeProperties: nameof(ApplicationUser.Avatar)
                ).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            if (request.DTO.Avatar != null)
            {
                _validator = new AndCondition(
                                new FileSizeValidator(request.DTO.Avatar, _config.GetMaxUploadSize()),
                                new FileHeaderValidator(request.DTO.Avatar)
                                );
                if (!_validator.Validate())
                {
                    throw new ValidationErrorException("There was a problem with the provided avatar");
                }
            }
            _validator = new EditUserValidator(request.DTO, _unitOfWork, request.User.GetUserIdFromJwt());
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Invalid modification");
            }
            userEntity.UserName = request.DTO.UserName;
            userEntity.FirstName = request.DTO.FirstName;
            userEntity.LastName = request.DTO.LastName;

            var avatar = request.DTO.Avatar;
            if (avatar != null)
            {
                var filePath = Path.GetTempFileName();
                var sanitizedFilename = _fileRepository.SanitizeFilename(avatar.FileName);
                var extension = Path.GetExtension(sanitizedFilename);
                using (var stream = File.Create(filePath))
                {
                    await avatar.CopyToAsync(stream, cancellationToken);
                }
                var savedImage = _fileRepository.SaveFile(Guid.Parse(request.User.GetUserIdFromJwt()), filePath, extension);
                userEntity.Avatar = savedImage;
            }

            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();

            var identityResult = await _userManager.SetUserNameAsync(userEntity, userEntity.UserName);
            if (identityResult.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', identityResult.Errors.Select(e => e.Description).ToList()));
            }
            return _mapper.Map<UserViewModel>(userEntity);
        }

        public async Task Handle(EditUserRoleCommand request, CancellationToken cancellationToken)
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
        }

        public async Task Handle(ClaimPartyCommand request, CancellationToken cancellationToken)
        {
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id == request.PartyBoughtDTO.UserId).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new ClaimValidator(RoleTypes.PartyExp, userEntity.Experience);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            var added = await _userManager.AddToRoleAsync(userEntity, RoleTypes.Party);
            if (added.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', added.Errors.Select(e => e.Description).ToList()));
            }
            userEntity.Experience -= RoleTypes.PartyExp;
            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();
        }

        public async Task Handle(ClaimDeckCommand request, CancellationToken cancellationToken)
        {
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id == request.DeckBoughtDTO.UserId).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new ClaimValidator(RoleTypes.GameExp, userEntity.Experience);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            _validator = new ClaimDeckValidator(userEntity.DeckClaims, request.DeckBoughtDTO.DeckType);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("User already has the game");
            }
            userEntity.DeckClaims.Add(request.DeckBoughtDTO.DeckType);
            userEntity.Experience -= RoleTypes.GameExp;
            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();
        }
    }
}
