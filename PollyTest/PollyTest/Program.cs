using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;

namespace PollyTest
{
    class Program
    {
        private const string EnvironmentVariablePrefix = "POLLY_TEST_";

        public static async Task Main(string[] args)
        {
            var host = CreateServiceHost(args);
            //await host.RunAsync();

            await host.Services.GetService<App>().Run();

        }

        private static IHost CreateServiceHost(string[] args)
        {
            var host = new HostBuilder()
                        .ConfigureHostConfiguration(hostConfig =>
                        {
                            hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                            hostConfig.AddJsonFile("hostsettings.json", true);
                            hostConfig.AddEnvironmentVariables(EnvironmentVariablePrefix);
                            hostConfig.AddCommandLine(args);
                        })
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                            config.AddJsonFile("appsettings.json", true, true);
                            config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true);
                        })
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddLogging(loggingBuilder =>
                            {
                                //loggingBuilder.AddConsole();
                            });

                            services.ConfigureServices(hostContext.Configuration);
                        })
                        .Build();
            return host;
        }

    }
}
