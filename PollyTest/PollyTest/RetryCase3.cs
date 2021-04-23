using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase3
    {
        public async Task Run()
        {
            await Polly.Policy
                .Handle<Exception>()
                .RetryAsync(3, (ex, i) => { 
                    Console.WriteLine($"{i} {ex}");
                })
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new Exception();
                });
        }
    }

}
