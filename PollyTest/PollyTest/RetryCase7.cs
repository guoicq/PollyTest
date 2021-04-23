using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class RetryCase7
    {
        public async Task Run()
        {
            var httpClient = new HttpClient();

            await Polly.Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, (i) =>
                {
                    return TimeSpan.FromSeconds(i);
                }, (ex, time) => {
                    Console.WriteLine($"{time} retry...");
                })
                .ExecuteAsync(async () => {
                    var response = await httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
                    response.EnsureSuccessStatusCode();
                    var version = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(version);
                });
        }
    }

}
