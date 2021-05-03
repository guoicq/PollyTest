using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace PollyTest
{
    public class BulkheadCase1
    {
        public async Task Run()
        {
            var tasks = new List<Task>();
            // Throttling 

            var bulkhead = Polly.Policy
                .BulkheadAsync(3, 10);

            for(var i=0; i<10; i++)
            {
                var t = bulkhead.ExecuteAsync(async () =>
                {
                    await Task.Delay(2000); 
                    Console.WriteLine($"{bulkhead.BulkheadAvailableCount}, {bulkhead.QueueAvailableCount}");
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);

        }
    }

}
