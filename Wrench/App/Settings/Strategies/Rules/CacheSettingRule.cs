using App.Settings.Strategies.Types;
using App.Validations;

namespace App.Settings.Strategies.Rules
{
    public class CacheSettingRule : IRule<CacheSetting>
    {
        public ValidateResult Do(CacheSetting value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.Id))
                result.AddError("Cache.Id is required");

            if(value.ProviderType == StrategyProviderType.None)
                result.AddError("Cache.ProviderType is required");

            if(value.ProviderType ==  StrategyProviderType.Redis)
            {
                var appSetting = ApplicationSetting.Current;
                if (appSetting?.Connections?.Redis == null)
                    result.AddError("Configure connection redis before use cache redis");
            }

            if(value.Ttl < 10)
            {
                result.AddError("Cache.Ttl should be bigger than 9 seconds");
            }

            return result;
        }
    }
}
