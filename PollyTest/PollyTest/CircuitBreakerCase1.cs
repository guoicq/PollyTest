using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class CircuitBreakerCase1
    {
        public async Task Run()
        {

            var httpClient = new HttpClient();

            var p = Polly.Policy
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(3));

            for (var i = 0; i < 1000; i++)
            {
                try
                {
                    await p.ExecuteAsync(async () => {
                        var response = await httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
                        response.EnsureSuccessStatusCode();
                        var version = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(version);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(1000);
            }
        }
    }

}
