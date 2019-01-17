using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Discovery;

namespace Nigel.Eureka.Faux
{
    public class BasicLifetimeHostedService: IHostedService
    {
        private readonly IServiceProvider _service;
        private readonly ILogger _logger;
        public BasicLifetimeHostedService(
            IServiceProvider service, ILogger<BasicLifetimeHostedService> logger)
        {
            _logger = logger;
            _service = service;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OnStarted has been called.");
            _service.GetRequiredService<IDiscoveryClient>();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OnStopping has been called.");
            return Task.CompletedTask;
        }
    }
}