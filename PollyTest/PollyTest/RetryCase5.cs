using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase5
    {
        public async Task Run()
        {
            await Polly.Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new HttpRequestException();
                });


            await Polly.Policy
                .Handle<Exception>(ex => !(ex is OperationCanceledException) && !(ex is ObjectDisposedException) )
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new HttpRequestException();
                });

            await Polly.Policy
                .Handle<Exception>(ex => !(ex is OperationCanceledException) && !(ex is ObjectDisposedException))
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new HttpRequestException();
                });

        }
    }

}
