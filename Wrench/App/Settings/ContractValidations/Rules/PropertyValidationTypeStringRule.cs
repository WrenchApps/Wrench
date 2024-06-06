
using App.Settings.ContractValidations.Types;
using App.Validations;

namespace App.Settings.ContractValidations.Rules
{

    public class PropertyValidationTypeStringRule : IRule<PropertyValidation>
    {
        public ValidateResult Do(PropertyValidation value)
        {
            var result = ValidateResult.Create();
            if (value.ValueType == PropertyValueType.String)
            {
                if (value.ValidationType == PropertyValidationType.BiggerThan ||
                    value.ValidationType == PropertyValidationType.LessThan)
                {
                    if (value.Length <= 0)
                        result.AddError("PropertyValidation.Length should be informed");
                }
            }

            return result;
        }
    }
}
