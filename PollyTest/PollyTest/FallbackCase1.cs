using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class FallbackCase1
    {
        public async Task Run()
        {
            var httpClient = new HttpClient();

            var r = await Polly.Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>( r => !r.IsSuccessStatusCode)
                .FallbackAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Not available")
                })
                .ExecuteAsync( () =>  httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke"));

            var s = await r.Content.ReadAsStringAsync();
            Console.WriteLine(s);
        }
    }

}
