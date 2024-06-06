using App.Settings.Strategies.Types;
using App.Validations;

namespace App.Settings.Strategies.Rules
{
    public class HttpIdempotencySettingRule : IRule<HttpIdempotencySetting>
    {
        public ValidateResult Do(HttpIdempotencySetting value)
        {
            var result = ValidateResult.Create();

            if(string.IsNullOrEmpty(value.Id))
            {
                result.AddError("HttpIdempotency.Id is required");
            }

            if (value.ProviderType == StrategyProviderType.None)
                result.AddError("HttpIdempotency.ProviderType is required");

            if (value.Ttl < 10)
            {
                result.AddError("HttpIdempotency.Ttl should be bigger than 9 seconds");
            }

            if (value.ProviderType == StrategyProviderType.Redis)
            {
                var appSetting = ApplicationSetting.Current;
                if (appSetting?.Connections?.Redis == null)
                    result.AddError("Configure connection redis before use cache redis");
            }

            return result;
        }
    }
}
