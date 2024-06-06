using App.Validations;

namespace App.Settings.ApiSecurets.ValidateRules
{
    public class ApiSecuritySettingTypeBasicRule : IRule<ApiSecuritySetting>
    {
        public ValidateResult Do(ApiSecuritySetting value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.User))
                result.AddError("ApiSecurity.User is required");

            if (string.IsNullOrEmpty(value.Password))
                result.AddError("ApiSecurity.Password is required");

            if (value.Type == Types.ApiSecurityType.None)
                result.AddError("ApiSecurity.Type is required");

            return result;
        }
    }
}
