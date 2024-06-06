using App.Validations;

namespace App.Settings.Connections.Rules
{
    public class RedisSettingRule : IRule<RedisSetting>
    {
        public ValidateResult Do(RedisSetting value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.ConnectionString))
                result.AddError("Redis.ConnectionString is required");

            return result;
        }
    }
}
