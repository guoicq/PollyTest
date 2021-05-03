using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase6
    {
        public async Task Run()
        {
            var n = 0;

            var p1 = Polly.Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                });


            var p2 = Polly.Policy
                .Handle<OperationCanceledException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"Operation Cancelled");
                });

            await p1.WrapAsync(p2)
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    var i = DateTime.Now.Millisecond;
                    if (i % 2 == 0)
                        throw new HttpRequestException();
                    else
                        throw new OperationCanceledException();
                });

        }
    }

}
