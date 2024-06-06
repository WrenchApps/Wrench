using App.Contexts;
using App.Handlers.Http;
using App.Settings.Entrypoints.Routes;
using Microsoft.AspNetCore.Authorization;

namespace App.Delegates
{
    public class HttpRequestDelegate
    {
        private RouteSetting _routeSetting;

        public HttpRequestDelegate(RouteSetting routeSetting) 
        {
            _routeSetting = routeSetting;
        }

        [Authorize]
        public async Task Do_Authorize(HttpContext context) => await Do(context);
     
        [AllowAnonymous]
        public async Task Do_Anonymous(HttpContext context) => await Do(context);


        private async Task Do(HttpContext context)
        {
            var stepladderContext = context.RequestServices.GetService<StepladderHttpContext>();
            stepladderContext.HttpContext = context;
            stepladderContext.RouteSetting = _routeSetting;

            var handler = HandlerChainBuilder.ChainBuilder(stepladderContext);
            await handler.DoAsync(stepladderContext);
        }
    }
}
