using App.Settings.MapVariables.Rules;
using App.Validations;

namespace App.Settings.MapVariables
{
    public class MapVariableSetting : IValidable
    {
        public string From { get; set; }
        public string To { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<MapVariableSetting>[]
            {
                new MapVariableSettingRule()
            };

            var result = RuleExecute.Execute(this, rules);

            return result;
        }
    }
}
