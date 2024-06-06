using App.Validations;

namespace App.Settings.ValidateRules
{
    public class StartupSettingRule : IRule<StartupSetting>
    {
        public ValidateResult Do(StartupSetting value)
        {
            var result = ValidateResult.Create();

            var httpClientAuthenticationDuplicateIds = value.HttpClientAuthentication?.GroupBy(p => p.Id)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (httpClientAuthenticationDuplicateIds?.Count > 0)
                result.AddError("Startup.HttpClientAuthentication.Id duplicated");

            if (string.IsNullOrEmpty(value.ServiceName))
                result.AddError("Startup.ServiceName is required");

            if (string.IsNullOrEmpty(value.ServiceVersion))
                result.AddError("Startup.ServiceVersion is required");

            if (value.AwsSecretEnable)
            {
                if (string.IsNullOrEmpty(value.Prefix))
                    result.AddError("Startup.Prefix is required");
            }

            if (value.EnableTelemetry)
            {
                if (string.IsNullOrEmpty(value.OtelEndpoint))
                    result.AddError("Startup.OtelEndpoint is required");
            }

            var mapVariablesDuplicateNames = value.MapVariables?.GroupBy(p => p.From)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (mapVariablesDuplicateNames?.Count > 0)
                result.AddError("Startup.MapVariables.Name duplicated");

            return result;
        }
    }
}
