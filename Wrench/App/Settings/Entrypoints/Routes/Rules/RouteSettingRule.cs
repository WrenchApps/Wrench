using App.Types;
using App.Validations;

namespace App.Settings.Entrypoints.Routes.Rules
{
    public class RouteSettingRule : IRule<RouteSetting>
    {
        public ValidateResult Do(RouteSetting value)
        {
            var result = ValidateResult.Create();

            if (value.Method == HttpMethodType.NONE)
                result.AddError("Route.Method is required");


            if (string.IsNullOrEmpty(value.Route))
                result.AddError("Route.Route is required");

            if (string.IsNullOrEmpty(value.FlowActionId) && value.ResponseMock == null)
                result.AddError("Route.FlowActionId is required");

            if (value.FlowActionId != null)
            {
                var appSetting = ApplicationSetting.Current;
                var hasFlowActionId =  appSetting.FlowActions?.FirstOrDefault(f => f.Id == value.FlowActionId) != null;

                if(hasFlowActionId is false)
                    result.AddError($"Route.FlowActionId {value.FlowActionId} should be configured");
            }


            return result;
        }
    }
}
