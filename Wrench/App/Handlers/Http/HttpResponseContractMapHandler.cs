using App.Contexts;
using App.Helpers;
using Opentelemetry.Trace;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace App.Handlers.Http
{
    public class HttpResponseContractMapHandler : Handler
    {
        public override async Task DoAsync(WrenchHttpContext context)
        {
            if (context.HasCache == false &&
                context.HasNoErrorProcessor &&
                context.ResponseContext.ResponseBodyStringValue != null &&
                ContractMap != null)
            {
                
                var trace = context.HttpContext.RequestServices.GetService<ITraceService>();
                Activity activity = null;
                if (trace != null)
                    activity = trace.StartActivity("HttpResponseContractMapHandler");

                var isArray = context.ResponseContext.CheckIfResponseIsArray();

                if (isArray)
                {
                    var jsonArray = context.ResponseContext.GetJsonArrayResponseBody();
                    
                    foreach (var json in jsonArray) 
                    {
                        var jsonObject = json as JsonObject;
                        if(jsonObject == null)
                            continue;

                        var jsonMapParse = new JsonMapParse(jsonObject, ContractMap);
                        jsonMapParse.MapParse();
                    }

                    context.ResponseContext.ResponseBodyStringValue = jsonArray.ToString();
                }
                else
                {
                    var json = context.ResponseContext.GetJsonResponseBody();
                    if (json != null)
                    {
                        var jsonMapParse = new JsonMapParse(json, ContractMap);
                        json = jsonMapParse.MapParse();
                        context.ResponseContext.ResponseBodyStringValue = json.ToString();
                    }
                }

                activity?.Dispose();
            }

            await NextAsync(context);
        }
    }
}
