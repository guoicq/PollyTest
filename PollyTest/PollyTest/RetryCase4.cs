using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase4
    {
        public async Task Run()
        {
            var n = 0;

            await Polly.Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(i);
                }, (ex, i) => {
                    Console.WriteLine($"{i} retry...");
                })
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new Exception();
                });
        }
    }

}
