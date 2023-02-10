﻿using MediatR;
using user.bll.Infrastructure;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;
using user.dal.Repository.Implementations;
using user.dal.Repository.Interfaces;
using user.dal.UnitOfWork.Implementations;
using user.dal.UnitOfWork.Interfaces;

namespace user.api.Extensions
{
    /// <summary>
    /// Helper class for adding services
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Add services for dependency injections
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceExtensions(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IRequestHandler<CreateUserCommand, bool>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserCommand, bool>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserRoleCommand, Unit>, UserCommandHandler>();

            services.AddTransient<IRequestHandler<GetUserQuery, UserViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>, UserQueryHandler>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}