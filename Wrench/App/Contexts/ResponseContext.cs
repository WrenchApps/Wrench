using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Contexts
{
    public class ResponseContext
    {
        private JsonObject _jsonResponseBody = null;
        private JsonArray _jsonArrayResponseBody = null;
        public JsonObject GetJsonResponseBody()
        {
            if(_jsonResponseBody == null && string.IsNullOrEmpty(ResponseBodyStringValue) == false)
                _jsonResponseBody = JsonSerializer.Deserialize<JsonObject>(ResponseBodyStringValue);

            return _jsonResponseBody;
        }

        public JsonArray GetJsonArrayResponseBody()
        {
            if (_jsonArrayResponseBody == null && string.IsNullOrEmpty(ResponseBodyStringValue) == false)
                _jsonArrayResponseBody = JsonSerializer.Deserialize<JsonArray>(ResponseBodyStringValue);

            return _jsonArrayResponseBody;
        }

        public bool CheckIfResponseIsArray()
        {
            var start = ResponseBodyStringValue.StartsWith('[');
            var end1 = ResponseBodyStringValue.EndsWith("]");
            var end2 = ResponseBodyStringValue.EndsWith("]\n");

            return start && (end1 || end2);
        }

        public string ResponseBodyStringValue { get; set; }
        public int ResponseStatusCode { get; set; }
        public string ResponseContentType { get; set; } = "application/json";
        public bool IsSuccessStatusCode { get; set; }
    }
}