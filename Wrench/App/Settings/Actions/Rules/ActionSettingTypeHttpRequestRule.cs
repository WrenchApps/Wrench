using App.Settings.Actions.Types;
using App.Types;
using App.Validations;

namespace App.Settings.Actions.Rules
{
    public class ActionSettingTypeHttpRequestRule : IRule<ActionSetting>
    {
        public ValidateResult Do(ActionSetting value)
        {
            var result = ValidateResult.Create();

            if (value.Type == ActionType.HttpRequest)
            {
                if (string.IsNullOrEmpty(value.Uri))
                    result.AddError("Action.Uri is required");
                else
                {
                    Uri uri;
                    Uri.TryCreate(value.Uri, UriKind.Absolute, out uri);
                    if (uri is null)
                        result.AddError("Action.Uri should a valid uri");
                }

                if (value.Method == HttpMethodType.NONE)
                    result.AddError("Action.Method is required");

                if(value.ReponseContractMapId != null)
                {
                    var appSetting = ApplicationSetting.Current;
                    var hasContractMap = appSetting?.ContractMaps?.Any(c => c.Id == value.ReponseContractMapId) ?? false;
                    if(hasContractMap == false)
                        result.AddError($"Action.ReponseContractMapId {value.ReponseContractMapId} should configured before use");
                }


                if (value.RequestContractValidationId != null)
                {
                    var appSetting = ApplicationSetting.Current;
                    var hasContractMap = appSetting?.ContractValidations?.Any(c => c.Id == value.RequestContractValidationId) ?? false;
                    if (hasContractMap == false)
                        result.AddError($"Action.RequestContractValidationId {value.RequestContractValidationId} should configured before use");

                    if (value.Method == HttpMethodType.GET ||
                        value.Method == HttpMethodType.DELETE)
                        result.AddError("Action.RequestContractValidationId only used in post, put and patch methods");
                }

                if(value.StrategyCacheId != null)
                {
                    var appSetting = ApplicationSetting.Current;
                    var hasStrategyCacheId = appSetting?.Strategies?.Caches?.Any(c => c.Id == value.StrategyCacheId) ?? false;
                    if (hasStrategyCacheId == false)
                        result.AddError($"Action.StrategyCacheId {value.StrategyCacheId} should configured before use");

                    if(value.Method != HttpMethodType.GET)
                        result.AddError("Action.StrategyCacheId only used in get methods");
                }

                if (value.StrategyHttpIdempotencyId != null)
                {
                    var appSetting = ApplicationSetting.Current;
                    var hasStrategyCacheId = appSetting?.Strategies?.HttpIdempotencies?.Any(c => c.Id == value.StrategyHttpIdempotencyId) ?? false;
                    if (hasStrategyCacheId == false)
                        result.AddError($"Action.StrategyHttpIdempotencyId {value.StrategyHttpIdempotencyId} should configured before use");

                    if (value.Method == HttpMethodType.GET)
                       result.AddError("Action.StrategyHttpIdempotencyId only used in post, put, patch and delete methods");
                }
            }

            return result;
        }
    }
}
