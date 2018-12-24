using Microsoft.Extensions.DependencyInjection;

namespace Nige.Eureka.ApiTransfer
{
    public static class ApiTransferServiceCollectionExtensions
    {
        public static void AddApiTransferServices(this IServiceCollection services)
        {
            services.AddSingleton<IApiTransferService, ApiTransferService>();
        }
    }
}