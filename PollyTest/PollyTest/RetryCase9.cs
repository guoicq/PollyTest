using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase9
    {
        public async Task Run()
        {
            var n = 0;
            var retryResult = await Polly.Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, ts, i, ctx) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAndCaptureAsync(async () => {
                    n++;
                    Console.WriteLine(n);
                    await Task.Delay(1000);
                    if ( n % 5 == 0) 
                        return n;
                    else 
                        throw new HttpRequestException();
                });

            if (retryResult.FinalException != null)
            {
                Console.WriteLine(retryResult.FinalException.Message);
            }
            else
            {
                Console.WriteLine(retryResult.Result); 
            }

        }
    }

}
