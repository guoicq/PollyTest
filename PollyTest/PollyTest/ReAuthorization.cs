using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace PollyTest
{
    public class ReAuthorization
    {
        public async Task Run()
        {

            var httpClient = new HttpClient();

            var p = Polly.Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(3, (httpResponseMessage, i) => {
                    if (httpResponseMessage.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        PerformReauthorization();
                });

            await p.ExecuteAsync(async () => {
                var response = await httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");
                return response;
            });
        }

        private void PerformReauthorization()
        {
            // Renew token ...
        }
    }

}
