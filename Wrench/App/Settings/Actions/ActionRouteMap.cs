using App.Settings.Actions.Rules;
using App.Settings.Actions.Types;
using App.Validations;

namespace App.Settings.Actions
{
    public class ActionRouteMap : IValidable
    {
        public string RouteKey { get; set; }
        public FromType FromType { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ActionRouteMap>[]
            {
                new ActionRouteMapRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
