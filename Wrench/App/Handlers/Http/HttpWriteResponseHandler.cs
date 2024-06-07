using App.Contexts;

namespace App.Handlers.Http
{
    public class HttpWriteResponseHandler : Handler
    {
        public override async Task DoAsync(WrenchHttpContext context)
        {
            context.HttpContext.Response.Headers.Add("Content-Type", context.ResponseContext.ResponseContentType);

            if (context.HasNoErrorProcessor && context.ResponseContext.ResponseBodyStringValue != null)
            {
                context.HttpContext.Response.StatusCode = context.ResponseContext.ResponseStatusCode;
                await context.HttpContext.Response.WriteAsync(context.ResponseContext.ResponseBodyStringValue);
            }
            else if (context.HasNoErrorProcessor == false)
            {
                context.HttpContext.Response.StatusCode = 400;
                await context.HttpContext.Response.WriteAsync(context.ResponseContext.ResponseBodyStringValue);
            }

            await NextAsync(context);
        }
    }
}
