using App.Validations;

namespace App.Settings.Actions.Rules
{
    public class ActionHeaderMapRule : IRule<ActionHeaderMap>
    {
        public ValidateResult Do(ActionHeaderMap value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.MapFromTo))
                result.AddError("ActionHeaderMap.MapFromTo is required");
            else
            {
                if (value.MapFromTo.Contains(' '))
                    result.AddError("ActionHeaderMap.MapFromTo not accept space");

                if (value.MapFromTo.Contains(':') == false)
                    result.AddError("ActionHeaderMap.MapFromTo is invalid the mapFromTo should splitted with one ':'");
            }

            if (value.FromType == Types.FromType.None)
                result.AddError("ActionHeaderMap.FromType is required");

            return result;
        }
    }
}
