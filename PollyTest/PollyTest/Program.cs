using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        static void Main(string[] args)
        {
            var provider = BuildServiceProvider();

            _ = provider.GetService<App>().Run();

            Console.ReadKey();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            // Setup configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                });

            services.AddHttpClient<IApiClient, ApiClient>( client => { 
                    client.BaseAddress = new Uri(configuration.GetValue<string>("ApiServer.Url"));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());


            ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<App>();
            return services;
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var p = HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .Or<BrokenCircuitException>()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 100));

            return p;

        }

        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            var p = HttpPolicyExtensions.HandleTransientHttpError()
                                      .CircuitBreakerAsync(3, TimeSpan.FromSeconds(3), 
                                      (dr, ts) => { 
                                          Console.WriteLine("OnBreak"); 
                                      }, () => { 
                                          Console.WriteLine("OnReset"); 
                                      });
            return p;

        }

    }
}
