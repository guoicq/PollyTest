using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace PollyTest
{
    public interface IApiClient
    {
        Task<string> GetPostAsync(CancellationToken token = default(CancellationToken));
    }

}
