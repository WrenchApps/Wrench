using App.Validations;

namespace App.Settings.ContractValidations.Rules
{
    public class PropertyValidationArrayObjectRule : IRule<PropertyValidationArrayObject>
    {
        public ValidateResult Do(PropertyValidationArrayObject value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.ArrayPropertyName))
                result.AddError("PropertyValidationArrayObject.ArrayPropertyName is required");
            else
            {
                if (value.ArrayPropertyName.Contains(' '))
                    result.AddError("PropertyValidationArrayObject.ArrayPropertyName not accept empty value");
            }

            if (value.Properties == null || value.Properties.Count == 0)
                result.AddError("PropertyValidationArrayObject.Properties is required");

            return result;
        }
    }
}
