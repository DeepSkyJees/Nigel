using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nige.Eureka.ApiTransfer;
using Nigel.Eureka.Faux;

namespace Nigel.GenericHost.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostAsync(args);
        }

        public static async Task CreateHostAsync(string[] args)
        {
            if (args.Length != 5) throw new ArgumentException(nameof(args));

            var keyValuePairList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Hello", args[0]),
            };


            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", true);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json", true, true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                    configApp.AddInMemoryCollection(keyValuePairList);

                    //configApp.AddCommandLine(args);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                    configLogging.AddLog4Net();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddNigelFaux(hostContext.Configuration);

                    services.AddNigelFaux<IApiTransferService, ApiTransferService>(hostContext.Configuration);
                })

                .Build();

            await host.RunAsync();
        }
    }
}
