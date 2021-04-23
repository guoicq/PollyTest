using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class CircuitBreakerCase2
    {
        public async Task Run()
        {

            var httpClient = new HttpClient();

            var p = Polly.Policy
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(3));

            var p2 = Polly.Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(1000, i => {
                    return TimeSpan.FromSeconds(1);
                }, (ex, ts) =>
                {
                    Console.WriteLine(ex.Message);
                });

            await p2.WrapAsync(p)
                .ExecuteAsync(async () => {
                    var response = await httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
                    response.EnsureSuccessStatusCode();
                    var version = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(version);
                });
        }
    }

}
