using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class App
    {
        public async Task Run()
        {
            Console.WriteLine(DateTime.Now.ToString());
            await Task.Delay(1000);
        }

    }

}
