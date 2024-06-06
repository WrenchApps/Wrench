using App.Settings.ContractValidations.Rules;
using App.Settings.ContractValidations.Types;
using App.Validations;

namespace App.Settings.ContractValidations
{
    public class PropertyValidation : IValidable
    {
        public string PropertyName { get; set; }
        public PropertyValueType ValueType { get; set; }
        public PropertyValidationType ValidationType { get; set; }

        /// <summary>
        /// used when type is string
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// used when type is int/float
        /// </summary>
        public int? Value { get; set; }


        public ValidateResult Valid()
        {
            var rules = new IRule<PropertyValidation>[]
            {
                new PropertyValidationRule(),
                new PropertyValidationTypeStringRule(),
                new PropertyValidationTypeIntFloatRule()
            };

            return RuleExecute.Execute(this, rules);
        }
    }
}
