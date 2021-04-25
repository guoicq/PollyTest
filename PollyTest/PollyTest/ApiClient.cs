using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace PollyTest
{
    public class ApiClient: IApiClient
    {
        private readonly HttpClient httpClient;
        public ApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetPost()
        {
            var response = await httpClient.GetAsync("random_joke");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }

}
