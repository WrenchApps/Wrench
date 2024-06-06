using App.Settings.Strategies.Rules;
using App.Settings.Strategies.Types;
using App.Validations;

namespace App.Settings.Strategies
{
    public class CacheSetting : IValidable
    {
        public string Id { get; set; }
        public StrategyProviderType ProviderType { get; set; }
        public int Ttl { get; set; } = 300;
        public ValidateResult Valid()
        {
            var rules = new IRule<CacheSetting>[]
             {
                new CacheSettingRule()
             };

            return RuleExecute.Execute(this, rules);
        }
    }
}
