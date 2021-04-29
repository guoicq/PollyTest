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
            var retryResult = await Polly.Policy
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(1);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAndCaptureAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    return 0;
                    throw new HttpRequestException();
                });

            if (retryResult.FinalException != null)
            {
                Console.WriteLine("Failed!");
            }
            else
            {
                Console.WriteLine(retryResult.Result); 
            }

        }
    }

}
