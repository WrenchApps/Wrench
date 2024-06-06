using App.Settings.ContractValidations.Types;
using App.Validations;

namespace App.Settings.ContractValidations.Rules
{
    public class PropertyValidationTypeIntFloatRule : IRule<PropertyValidation>
    {
        public ValidateResult Do(PropertyValidation value)
        {
            var result = ValidateResult.Create();
            if (value.ValueType == PropertyValueType.Int || 
                value.ValueType == PropertyValueType.Float)
            {
                if (value.ValidationType == PropertyValidationType.BiggerThan ||
                    value.ValidationType == PropertyValidationType.LessThan)
                {
                    if (value.Value == null)
                        result.AddError("PropertyValidation.Value should be informed");
                }
            }

            return result;
        }
    }
}
