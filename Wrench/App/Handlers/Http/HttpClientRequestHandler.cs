using App.Contexts;
using App.Helpers;
using App.HttpRequests;
using App.Settings.Actions;

namespace App.Handlers.Http
{
    public class HttpClientRequestHandler : Handler
    {
        public override async Task DoAsync(WrenchHttpContext context)
        {
            if (context.HasCache == false && context.HasNoErrorProcessor && ActionSetting != null)
            {
                var httpRequestClient = context.HttpContext.RequestServices.GetService<HttpRequestClient>();

                var mappedHeaders = HttpHelper.LoadMapHeaders(context, ActionSetting);
                httpRequestClient.SetHeaders(mappedHeaders);
                httpRequestClient.SetToHttpClientPropagatedHeaders(context.HttpContext.Request.Headers);

                var uri = BuildFinalHttpClientUri(context);
                var jsonBody = await context.GetCurrentBodyToRequestStringAsync();

                var httpResponseMessage = await httpRequestClient.SendAsync(uri, ActionSetting.Method, jsonBody);
                await LoadHttpResponseMessageAsync(context, httpResponseMessage);
            }

            await NextAsync(context);
        }

        private string BuildFinalHttpClientUri(WrenchHttpContext context)
        {
            var httpClientUri = ActionSetting.Uri;
            if (ActionSetting.RouteMaps?.Count > 0)
            {
                foreach (var routeMap in ActionSetting.RouteMaps)
                {
                    var value = GetRouteValueFromHttpRequest(context, routeMap.RouteKey);
                    httpClientUri = httpClientUri.Replace(routeMap.RouteKey, value);
                }
            }

            if(ActionSetting.Method == Types.HttpMethodType.GET)
                httpClientUri = $"{httpClientUri}{context.HttpContext.Request.QueryString}";

            return httpClientUri;
        }

        private string GetRouteValueFromHttpRequest(WrenchHttpContext context, string key)
        {
            key = key.Replace("{", "").Replace("}", "");

            context.HttpContext.Request.RouteValues.TryGetValue(key, out var value);
            return value.ToString();
        }
    }
}
