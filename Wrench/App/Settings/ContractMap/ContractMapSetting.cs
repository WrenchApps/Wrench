using App.Settings.ContractMap.Rules;
using App.Validations;

namespace App.Settings.ContractMap
{
    public class ContractMapSetting : IValidable
    {
        public string Id { get; set; }
        public List<string> MapFromTo { get; set; }
        public List<string> Remove { get; set; }
        public List<ContractMapArray> MapArray { get; set; } = new List<ContractMapArray>();

        public ValidateResult Valid()
        {
            var rules = new IRule<ContractMapSetting>[]
            {
                new ContractMapSettingRule(),
            };

            var result =  RuleExecute.Execute(this, rules);
            foreach(var map in MapArray)
                result.Concate(map.Valid());

            return result;
        }
    }
}
