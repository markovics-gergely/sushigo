﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using user.bll.Infrastructure.Commands;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;
using user.bll.Validators.Interfaces;
using user.bll.Validators.Implementations;
using user.bll.Exceptions;
using user.bll.Extensions;
using user.dal.Types;

namespace user.bll.Infrastructure
{
    public class UserCommandHandler :
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<EditUserCommand, bool>,
        IRequestHandler<ClaimPartyCommand, Unit>,
        IRequestHandler<ClaimGameCommand, Unit>,
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

        public async Task<Unit> Handle(ClaimPartyCommand request, CancellationToken cancellationToken)
        {
            _validator = new ClaimValidator(RoleTypes.PartyExp, request.User?.GetUserExpFromJwt() ?? 0);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            if (request.User == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id.ToString() == request.User.GetUserIdFromJwt()).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var added = await _userManager.AddToRoleAsync(userEntity, RoleTypes.Party);
            if (added.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', added.Errors.Select(e => e.Description).ToList()));
            }
            userEntity.Experience -= RoleTypes.PartyExp;
            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(ClaimGameCommand request, CancellationToken cancellationToken)
        {
            _validator = new ClaimValidator(RoleTypes.GameExp, request.User?.GetUserExpFromJwt() ?? 0);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            if (request.User == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var userEntity = _unitOfWork.UserRepository.Get(filter: x => x.Id.ToString() == request.User.GetUserIdFromJwt()).FirstOrDefault();
            if (userEntity == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new ClaimGameValidator(userEntity.GameClaims, request.GameType);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("User already has the game");
            }
            userEntity.GameClaims.Add(request.GameType);
            userEntity.Experience -= RoleTypes.GameExp;
            _unitOfWork.UserRepository.Update(userEntity);
            await _unitOfWork.Save();
            return Unit.Value;
        }
    }
}
