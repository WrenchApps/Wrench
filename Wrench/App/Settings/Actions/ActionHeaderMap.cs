using App.Settings.Actions.Rules;
using App.Settings.Actions.Types;
using App.Validations;

namespace App.Settings.Actions
{
    public class ActionHeaderMap : IValidable
    {
        public string MapFromTo { get; set; }
        public FromType FromType { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ActionHeaderMap>[]
            {
                new ActionHeaderMapRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}