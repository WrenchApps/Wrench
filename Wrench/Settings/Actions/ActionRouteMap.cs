using Wrench.Validations;

namespace Wrench.Settings.Actions
{
    public class ActionRouteMap : IValidable
    {
        public string RouteKey { get; set; }
        public string CurrentBodyKey { get; set; }
        public string JwtTokenClaimKey { get; set; }


        public ValidateResult Valid()
        {
            var rules = new IRule<ActionRouteMap>[]
            {
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
