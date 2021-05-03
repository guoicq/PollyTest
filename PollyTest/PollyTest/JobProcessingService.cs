using System;
using System.IO;
using System.Net.Http;
using System.Threading;
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
    public class JobProcessingService : IHostedService, IDisposable
    {
        private CancellationTokenSource cts;
        private Task currentTask;

        private readonly IApiClient apiClient;
        public JobProcessingService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            currentTask = apiClient.GetPostAsync(cts.Token);

            return currentTask.IsCompleted ? currentTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

            if (currentTask == null)
            {
                return;
            }

            try
            {
                cts.Cancel();
            }
            finally
            {
                await Task.WhenAny(currentTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            cts.Cancel();
        }

    }
}
