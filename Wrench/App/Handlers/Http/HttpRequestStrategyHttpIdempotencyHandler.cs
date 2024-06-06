using App.Contexts;
using App.Helpers;
using App.Settings;
using RedLockNet.SERedis;
using System.Text;

namespace App.Handlers.Http
{
    public class HttpRequestStrategyHttpIdempotencyHandler : Handler
    {
        private string _idempPrefix = $"{ApplicationSetting.Current.Startup.ServiceName}:{ApplicationSetting.Current.Startup.ServiceVersion}:Idemp";

        private static TimeSpan EXPIRY = TimeSpan.FromSeconds(15);
        private static TimeSpan WAIT = TimeSpan.FromSeconds(10);
        private static TimeSpan RETRY = TimeSpan.FromMilliseconds(100);

        public override async Task DoAsync(StepladderHttpContext context)
        {
            if (context.HasNoErrorProcessor)
            {
                var redLockFactory = context.HttpContext.RequestServices.GetService<RedLockFactory>();
                StringBuilder headerValuesConcated = new StringBuilder();
                foreach (var headerKey in HttpIdempotencySetting.MapHeaderProperties)
                {
                    var headerValue = HttpHelper.GetHeaderValue(context.HttpContext.Request.Headers, headerKey);
                    headerValuesConcated.Append(headerValue);
                }
                var headerValues = headerValuesConcated.ToString();
                var resource = $"{_idempPrefix}:{headerValues}";
                context.SufixCache = headerValues;

                try
                {
                    using var redLock = await redLockFactory.CreateLockAsync(resource, EXPIRY, WAIT, RETRY);
                    if (redLock.IsAcquired)
                    {
                        await NextAsync(context);
                    }
                    else
                    {
                        SetErrorProcessor(context);
                        await NextAsync(context);
                    }
                }
                catch
                {
                    SetErrorProcessor(context);
                    await NextAsync(context);
                }
            }
            else
                await NextAsync(context);
        }

        private void SetErrorProcessor(StepladderHttpContext context)
        {
            context.SetHttpProcessorWithError();
            context.ResponseContext.ResponseStatusCode = 503;
            context.ResponseContext.IsSuccessStatusCode = false;
        }
    }
}
