using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class Injection1
    {
        private readonly IApiClient apiClient;
        public Injection1(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task Run()
        {
            var result = await apiClient.GetPost();
            Console.WriteLine(result);
        }

    }

}
