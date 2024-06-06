using App.Settings.Actions.Rules;
using App.Validations;

namespace App.Settings.Actions
{
    public class FlowActionsSetting : IValidable
    {
        public string Id { get; set; }
        public List<string> ActionsId { get; set; }


        public ValidateResult Valid()
        {
            var rules = new IRule<FlowActionsSetting>[]
            {
                new FlowActionsSettingRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
