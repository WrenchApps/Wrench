using App.Settings.ApiSecurets.Types;
using App.Settings.ApiSecurets.ValidateRules;
using App.Validations;

namespace App.Settings.ApiSecurets
{
    public class ApiSecuritySetting : IValidable
    {
        public ApiSecurityType Type { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ApiSecuritySetting>[]
            {
                new ApiSecuritySettingTypeBasicRule(),
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
