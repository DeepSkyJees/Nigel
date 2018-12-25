// Copyright (c) Arch team. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nige.EntityFrameworkCore.UnitOfWork
{
    /// <summary>
    ///     Extension methods for setting up unit of work related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class UnitOfWorkServiceCollectionExtensions
    {
        /// <summary>
        ///     Registers the unit of work given context as a service in the <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TContext">The type of the db context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        /// <remarks>This method only support one db context, if been called more than once, will throw exception.</remarks>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services,
            Action<UnitOfWorkOptions> setupAction = null)
            where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

            services.ConfigUnitOfWorkOptions(setupAction);

            return services;
        }


        /// <summary>
        ///     Configurations the unit of work options.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        private static void ConfigUnitOfWorkOptions(this IServiceCollection services,
            Action<UnitOfWorkOptions> setupAction)
        {
            if (setupAction == null)
                setupAction = options => { options.DatabaseType = DatabaseType.SqlServer; };
            services.Configure(setupAction);
        }
    }
}