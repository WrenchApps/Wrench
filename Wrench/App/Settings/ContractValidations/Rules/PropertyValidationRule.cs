
using App.Settings.ContractValidations.Types;
using App.Validations;

namespace App.Settings.ContractValidations.Rules
{

    public class PropertyValidationRule : IRule<PropertyValidation>
    {
        public ValidateResult Do(PropertyValidation value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.PropertyName))
                result.AddError("PropertyValidation.PropertyName is required");
            else
            {
                if (value.PropertyName.Contains(' '))
                    result.AddError("PropertyValidation.PropertyName not accept space");
            }

            if (value.ValidationType == PropertyValidationType.None)
                result.AddError("PropertyValidation.ValidationType is required");


            if (value.ValueType == PropertyValueType.None)
                result.AddError("PropertyValidation.ValueType is required");

            return result;
        }
    }
}
