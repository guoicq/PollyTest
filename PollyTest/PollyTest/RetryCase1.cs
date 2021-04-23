using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase1
    {
        public async Task Run()
        {
            await Polly.Policy
                .Handle<Exception>()
                .RetryAsync()
                .ExecuteAsync(async () => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new Exception();
                });
        }
    }

}
