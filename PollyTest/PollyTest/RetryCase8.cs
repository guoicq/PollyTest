using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase8
    {
        public async Task Run()
        {
            var ts = new CancellationTokenSource();
            _ = CancelIn2Second(ts);

            await Polly.Policy
                .Handle<Exception>()
                .RetryAsync(3)
                .ExecuteAsync(async (token) => {
                    Console.WriteLine(DateTime.Now.ToString());
                    await Task.Delay(1000);
                    throw new Exception();
                }, ts.Token)
                .ContinueWith(t => Console.WriteLine("Continue"));

        }

        private async Task CancelIn2Second(CancellationTokenSource ts)
        {
            await Task.Delay(2000);
            ts.Cancel();
        }

    }

}
