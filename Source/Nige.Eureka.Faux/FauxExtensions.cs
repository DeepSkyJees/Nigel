using System;
using DHaven.Faux;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nigel.Eureka.Faux;

namespace Nigel.Eureka.Faux
{
    public static class FauxExtensions
    {

        /// <summary>
        /// Adds the hua GUI faux.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddNigelFaux<TService, TImplementation>(this IServiceCollection services, IConfiguration configuration) where TService : class where TImplementation : class, TService
        {
            services.AddFaux(configuration);
            services.AddSingleton<TService, TImplementation>();
            services.AddHostedService<BasicLifetimeHostedService>();
            return services;
        }

        /// <summary>
        /// Adds the hua GUI faux.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddNigelFaux(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFaux(configuration);
            services.AddHostedService<BasicLifetimeHostedService>();
            return services;
        }
    }
}
