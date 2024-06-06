using App.Settings.Actions.Types;
using App.Validations;

namespace App.Settings.Actions.Rules
{
    public class ActionRouteMapRule : IRule<ActionRouteMap>
    {
        public ValidateResult Do(ActionRouteMap value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.RouteKey))
                result.AddError("ActionRouteMap.RouteKey is required");

            if (value.FromType == FromType.None)
                result.AddError("ActionRouteMap.FromType is required");

            return result;
        }
    }
}
