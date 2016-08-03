using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace marvel_api.Adapters
{
    public class HttpClientAdapter : IHttpClientAdapter
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMilliseconds(1000);

        private readonly HttpClient _httpClient;

        public HttpClientAdapter()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = _defaultTimeout;
        }

        public void SetDefaultHeaders(IList<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        public async Task<JObject> GetAsync(Uri requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl);

            if(response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseBody);
            }

            return new JObject();
        }

        public void Dispose()
        {
            if(_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
    }
}