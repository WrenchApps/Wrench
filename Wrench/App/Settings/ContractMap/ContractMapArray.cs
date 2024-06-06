using App.Settings.ContractMap.Rules;
using App.Validations;

namespace App.Settings.ContractMap
{
    public class ContractMapArray : IValidable
    {
        public string ArrayMapFromTo { get; set; }
        public List<string> MapFromTo { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ContractMapArray>[]
             {
                new ContractMapArrayRule(),
             };

            return RuleExecute.Execute(this, rules);
        }
    }
}
