using App.Delegates;
using App.Handlers.Http.FlowActions;
using App.Settings;
using App.Types;
using System.Text;

namespace App.Extensions
{
    public static class LoadEntrypointsExtensions
    {
        private static StringBuilder ENDPOINT_LOADED = new StringBuilder();

        public static void UseConfigRoutes(this WebApplication app)
        {
            var appConfig = ApplicationSetting.Current;

            if (appConfig.Entrypoints?.Routes?.Count() > 0)
            {
                foreach (var route in appConfig.Entrypoints.Routes)
                {
                    var info = $"STARTED ROUTE: {route.Route} METHOD: {route.Method}";
                    ENDPOINT_LOADED.AppendLine(info);

                    var requestDelegate = new HttpRequestDelegate(route);
                    var enabledAnonymous = appConfig.Startup.HasApiSecurity == false || route.EnableAnonymous == true;

                    if (route.Method == HttpMethodType.GET)
                        app.MapGet(route.Route, enabledAnonymous ? requestDelegate.Do_Anonymous : requestDelegate.Do_Authorize);
                    else if (route.Method == HttpMethodType.POST)
                        app.MapPost(route.Route, enabledAnonymous ? requestDelegate.Do_Anonymous : requestDelegate.Do_Authorize);
                    else if (route.Method == HttpMethodType.PUT)
                        app.MapPut(route.Route, enabledAnonymous ? requestDelegate.Do_Anonymous : requestDelegate.Do_Authorize);
                    else if (route.Method == HttpMethodType.PATCH)
                        app.MapPatch(route.Route, enabledAnonymous ? requestDelegate.Do_Anonymous : requestDelegate.Do_Authorize);
                    else if (route.Method == HttpMethodType.DELETE)
                        app.MapDelete(route.Route, enabledAnonymous ? requestDelegate.Do_Anonymous : requestDelegate.Do_Authorize);

                    EntrypointHttpFlowActionsChainBuilder.Builder(route);

                }
            }

            app.MapGet("/", () => ENDPOINT_LOADED.ToString());
        }
    }
}
