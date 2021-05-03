using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase0
    {
        public async Task Run()
        {
            var n = 0;
            Console.WriteLine(n);
            await Task.Delay(1000);
        }
    }

}
