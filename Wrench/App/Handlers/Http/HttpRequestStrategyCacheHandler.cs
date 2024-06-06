using App.Contexts;
using App.Settings;
using StackExchange.Redis;
using System.Text.Json;

namespace App.Handlers.Http
{
    public class HttpRequestStrategyCacheHandler : Handler
    {
        private string _cachePrefix = $"{ApplicationSetting.Current.Startup.ServiceName}:{ApplicationSetting.Current.Startup.ServiceVersion}:Cache";

        public override async Task DoAsync(StepladderHttpContext context)
        {
            if (context.HasNoErrorProcessor)
            {
                var database = context.HttpContext.RequestServices.GetService<IDatabase>();
                var cacheKey = $"{_cachePrefix}:{context.HttpContext.Request.Path}:{context.SufixCache}";
                var cacheValueString = await database.StringGetAsync(cacheKey, CommandFlags.PreferReplica);

                if (string.IsNullOrEmpty(cacheValueString) == false)
                {
                    context.HasCache = true;
                    var cacheValue = JsonSerializer.Deserialize<ResponseContext>(cacheValueString);
                    context.ResponseContext.ResponseBodyStringValue = cacheValue.ResponseBodyStringValue;
                    context.ResponseContext.ResponseStatusCode = cacheValue.ResponseStatusCode;
                    context.ResponseContext.IsSuccessStatusCode = cacheValue.IsSuccessStatusCode;
                }

                await NextAsync(context);

                if (context.ResponseContext.IsSuccessStatusCode && context.HasCache == false)
                {
                    var cacheValue = new ResponseContext
                    {
                        ResponseBodyStringValue = context.ResponseContext.ResponseBodyStringValue,
                        ResponseStatusCode = context.ResponseContext.ResponseStatusCode,
                        IsSuccessStatusCode = context.ResponseContext.IsSuccessStatusCode
                    };

                    cacheValueString = JsonSerializer.Serialize(cacheValue);
                    await database.StringSetAsync(cacheKey, cacheValueString, TimeSpan.FromSeconds(CacheSetting.Ttl), flags: CommandFlags.FireAndForget);
                }
            }
            else
                await NextAsync(context);
        }
    }
}
