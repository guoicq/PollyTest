using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace PollyTest
{
    public interface IApiClient
    {
        Task<string> GetPost();
    }

}
