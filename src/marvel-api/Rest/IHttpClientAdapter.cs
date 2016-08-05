using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Rest
{
    public interface IHttpClientAdapter : IDisposable
    {
        void SetDefaultHeaders(IList<KeyValuePair<string, string>> headers);

        Task<JObject> GetAsync(Uri requestUrl);
    }
}