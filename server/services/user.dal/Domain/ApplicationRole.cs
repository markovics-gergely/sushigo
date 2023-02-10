﻿using Microsoft.AspNetCore.Identity;

namespace user.dal.Domain
{
    /// <summary>
    /// Roles of users
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
    }
}
