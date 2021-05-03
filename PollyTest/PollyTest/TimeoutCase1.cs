using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace PollyTest
{
    public class TimeoutCase1
    {
        public async Task Run()
        {

            var r = await Polly.Policy
                .TimeoutAsync(5)
                .ExecuteAsync(async (ct) => { await Task.Delay(3, ct); return 0; }, CancellationToken.None);

            Console.WriteLine(r);
        }
    }

}
