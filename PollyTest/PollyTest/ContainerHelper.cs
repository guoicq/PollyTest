using System;
using System.IO;
using System.Net.Http;
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
    public static class ContainerHelper
    {

        public static ServiceProvider ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Setup injection
            services.AddHttpClient<IApiClient, ApiClient>(client =>
                {
                    client.BaseAddress = new Uri(configuration.GetValue<string>("ApiServer.Url"));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());


            services.AddScoped<App>();
            services.AddHostedService<JobProcessingService>();

            return services.BuildServiceProvider();
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
                        (dr, ts) =>
                        {
                            Console.WriteLine("OnBreak");
                        }, () =>
                        {
                            Console.WriteLine("OnReset");
                        });
            return p;

        }

    }
}
