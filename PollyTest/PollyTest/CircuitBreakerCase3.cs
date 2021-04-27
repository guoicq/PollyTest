using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class CircuitBreakerCase3
    {
        public async Task Run()
        {

            var httpClient = new HttpClient();

            var p = Polly.Policy
                .HandleResult<HttpResponseMessage>( r => !r.IsSuccessStatusCode)
                .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(60), 5, TimeSpan.FromSeconds(10));

            var p2 = Polly.Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(1000, i => {
                    return TimeSpan.FromSeconds(1);
                }, (ex, ts) =>
                {
                    Console.WriteLine(ex.Message);
                });

            var response = await p2.WrapAsync(p)
                .ExecuteAsync(async () => {
                    var response = await httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
                    return response;
                });

            var version = await response.Content.ReadAsStringAsync();
            Console.WriteLine(version);

        }
    }

}
