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
        private readonly IApiClient apiClient;
        public App(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task Run()
        {
            //var result = await apiClient.GetPostAsync();
            //Console.WriteLine(result);
            await new RetryCase1().Run();
        }

    }

}
