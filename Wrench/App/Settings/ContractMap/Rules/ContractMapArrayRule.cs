using App.Validations;

namespace App.Settings.ContractMap.Rules
{
    public class ContractMapArrayRule : IRule<ContractMapArray>
    {
        public ValidateResult Do(ContractMapArray value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.ArrayMapFromTo))
                result.AddError("ContractMapArray.ArrayMapFromTo is required");

            if (value.MapFromTo?.Count > 0)
            {
                foreach (var map in value.MapFromTo)
                {
                    if (map.Contains(' '))
                        result.AddError("ContractMapArray.MapFromTo not accept space");

                    if (map.Contains('.'))
                        result.AddError("ContractMapArray.MapFromTo not accept '.'");
                }
            }

            return result;
        }
    }
}
