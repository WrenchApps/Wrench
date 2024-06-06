using App.Settings.Connections.Rules;
using App.Validations;

namespace App.Settings.Connections
{
    public class RedisSetting : IValidable
    {
        public string ConnectionString { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<RedisSetting>[]
            {
                new RedisSettingRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
