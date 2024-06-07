using App.Settings.ContractValidations.Rules;
using App.Validations;

namespace App.Settings.ContractValidations
{
    public class PropertyValidationArrayObject : IValidable
    {
        public string ArrayPropertyName { get; set; }
        public List<PropertyValidation> Properties { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<PropertyValidationArrayObject>[]
            {
                new PropertyValidationArrayObjectRule(),
            };

            var result = RuleExecute.Execute(this, rules);

            foreach (var Property in Properties)
                result.Concate(Property.Valid());

            return result;
        }
    }
}