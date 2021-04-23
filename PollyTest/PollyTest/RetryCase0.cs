using System;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase0
    {
        public async Task Run()
        {
            Console.WriteLine(DateTime.Now.ToString());
            await Task.Delay(1000);
        }
    }

}
