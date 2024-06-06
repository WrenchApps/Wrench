using App.Validations;

namespace App.Settings.ContractValidations.Rules
{
    public class ContractValidationRule : IRule<ContractValidation>
    {
        public ValidateResult Do(ContractValidation value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.Id))
                result.AddError("ContractValidation.Id is required");

            return result;
        }
    }
}
