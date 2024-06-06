using App.Validations;

namespace App.Settings.Strategies.Rules
{
    public class StrategiesSettingRule : IRule<StrategiesSetting>
    {
        public ValidateResult Do(StrategiesSetting value)
        {
            var result = ValidateResult.Create();

            if (value.Caches != null)
            {
                foreach (var cache in value.Caches)
                {
                    var duplicated = value.Caches.Count(c => c.Id == cache.Id) > 1;
                    if (duplicated)
                        result.AddError($"Strategies.Caches.Id {cache.Id} is duplicated");
                }
            }

            if (value.HttpIdempotencies != null)
            {
                foreach (var idemp in value.HttpIdempotencies)
                {
                    var duplicated = value.HttpIdempotencies.Count(i => i.Id == idemp.Id) > 1;
                    if (duplicated)
                        result.AddError($"Strategies.HttpIdempotencies.Id {idemp.Id} is duplicated");
                }
            }

            return result;
        }
    }
}
