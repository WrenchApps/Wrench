using App.Settings.Strategies.Rules;
using App.Validations;

namespace App.Settings.Strategies
{
    public class StrategiesSetting : IValidable
    {
        public List<CacheSetting> Caches { get; set; }
        public List<HttpIdempotencySetting> HttpIdempotencies { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<StrategiesSetting>[]
            {
                new StrategiesSettingRule()
            };

            var result = RuleExecute.Execute(this, rules);

            if (Caches != null)
                foreach (var cache in Caches)
                    result.Concate(cache.Valid());

            if (HttpIdempotencies != null)
                foreach (var idemp in HttpIdempotencies)
                    result.Concate(idemp.Valid());

            return result;
        }
    }
}
