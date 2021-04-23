using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase2
    {
        public async Task Run()
        {
            await Polly.Policy
                .Handle<Exception>()
                .RetryAsync(3)
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new Exception();
                });

        }
    }

}
