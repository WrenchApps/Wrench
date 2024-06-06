
using App.Helpers;
using App.Types;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.HttpRequests
{
    public class HttpRequestClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpRequestClient> _logger;
        private readonly HttpClient _httpClient;

        public HttpRequestClient(IHttpClientFactory httpClientFactory, ILogger<HttpRequestClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public void SetToHttpClientPropagatedHeaders(IHeaderDictionary headers)
            => HttpHelper.SetToHttpClientPropagatedHeaders(headers, _httpClient);


        public void SetHeaders(IDictionary<string, string> headers)
        {
            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        public async Task<HttpResponseMessage> SendAsync(string uri, HttpMethodType httpMethodType, string stringBody = null)
        {
            if (httpMethodType == HttpMethodType.GET)
                return await _httpClient.GetAsync(uri);
            else if (httpMethodType == HttpMethodType.POST)
            {
                var jsonObject = ParseBodyString(stringBody);
                return await _httpClient.PostAsJsonAsync(uri, jsonObject);
            }
            else if (httpMethodType == HttpMethodType.PUT)
            {
                var jsonObject = ParseBodyString(stringBody);
                return await _httpClient.PutAsJsonAsync(uri, jsonObject);
            }
            else if (httpMethodType == HttpMethodType.PATCH)
            {
                var jsonObject = ParseBodyString(stringBody);
                return await _httpClient.PatchAsJsonAsync(uri, jsonObject);
            }
            else if (httpMethodType == HttpMethodType.DELETE)
            {
                return await _httpClient.DeleteAsync(uri);
            }

            return null;
        }

        private JsonObject ParseBodyString(string stringBody)
        {
            JsonObject jsonObject = null;
            if (string.IsNullOrEmpty(stringBody) == false)
                jsonObject = JsonSerializer.Deserialize<JsonObject>(stringBody);

            return jsonObject;  
        }
    }
}
