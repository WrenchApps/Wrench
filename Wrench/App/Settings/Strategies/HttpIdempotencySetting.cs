using App.Settings.Strategies.Rules;
using App.Settings.Strategies.Types;
using App.Validations;

namespace App.Settings.Strategies
{
    public class HttpIdempotencySetting : IValidable
    {
        public string Id { get; set; }
        public int Ttl { get; set; } = 300; 
        public StrategyProviderType ProviderType { get; set; }
        public List<string> MapHeaderProperties { get; set; }


        public ValidateResult Valid()
        {
            var rules = new IRule<HttpIdempotencySetting>[]
            {
                new HttpIdempotencySettingRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
