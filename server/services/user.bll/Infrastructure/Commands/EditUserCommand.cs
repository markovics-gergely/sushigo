﻿using MediatR;
using System.Security.Claims;
using user.bll.Infrastructure.DataTransferObjects;

namespace user.bll.Infrastructure.Commands
{
    public class EditUserCommand : IRequest<bool>
    {
        public EditUserDTO DTO { get; set; }
        public ClaimsPrincipal? User { get; set; }

        public EditUserCommand(EditUserDTO dto, ClaimsPrincipal? user = null)
        {
            DTO = dto;
            User = user;
        }
    }
}
