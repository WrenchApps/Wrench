using App.Validations;

namespace App.Settings.MapVariables.Rules
{
    public class MapVariableSettingRule : IRule<MapVariableSetting>
    {
        public ValidateResult Do(MapVariableSetting value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.From))
                result.AddError("MapVariable.From is required");

            if(string.IsNullOrEmpty(value.To))
                result.AddError("MapVariable.To is required");

            return result;
        }
    }
}
