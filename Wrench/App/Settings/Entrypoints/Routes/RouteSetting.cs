using App.Settings.Entrypoints.Routes.Rules;
using App.Types;
using App.Validations;

namespace App.Settings.Entrypoints.Routes
{
    public class RouteSetting : IValidable
    {
        public string Route { get; set; }
        public HttpMethodType Method { get; set; }
        public bool EnableAnonymous { get; set; }
        public string FlowActionId { get; set; }
        public ResponseMock ResponseMock { get; set; }

        public string GetFlowActionId => FlowActionId ?? Route;

        public ValidateResult Valid()
        {
            var rules = new IRule<RouteSetting>[]
            {
                new RouteSettingRule(),
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
