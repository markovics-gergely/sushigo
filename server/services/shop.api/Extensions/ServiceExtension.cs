﻿using MediatR;
using shared.api.Extensions;
using shared.dal.Repository.Implementations;
using shared.dal.Repository.Interfaces;
using shop.bll.Infrastructure;
using shop.bll.Infrastructure.Commands;
using shop.bll.Infrastructure.Queries;
using shop.bll.Infrastructure.ViewModels;
using shop.dal;
using shop.dal.UnitOfWork.Implementations;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.api.Extensions
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
            // Shared Services
            services.AddSharedServiceExtensions();

            // Shop Commands
            services.AddTransient<IRequestHandler<BuyDeckCommand>, ShopCommandHandler>();
            services.AddTransient<IRequestHandler<BuyPartyCommand>, ShopCommandHandler>();

            // Shop Queries
            services.AddTransient<IRequestHandler<GetDecksQuery, IEnumerable<DeckViewModel>>, ShopQueryHandler>();
            services.AddTransient<IRequestHandler<GetDeckQuery, DeckItemViewModel>, ShopQueryHandler>();

            // Providers
            services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider<ShopDbContext>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
