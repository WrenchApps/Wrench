using App.Validations;

namespace App.Settings.Connections
{
    public class ConnectionSetting: IValidable
    {
        public RedisSetting Redis { get; set; }

        public ValidateResult Valid()
        {
            var result = ValidateResult.Create();

            result.Concate(Redis.Valid());

            return result;
        }
    }
}
