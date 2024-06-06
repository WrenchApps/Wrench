using App.Settings.ContractValidations.Rules;
using App.Validations;

namespace App.Settings.ContractValidations
{
    public class ContractValidation : IValidable
    {
        public string Id { get; set; }
        public List<PropertyValidation> Properties { get; set; }
        public List<PropertyValidationArrayObject> ValidationArrayObjects { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ContractValidation>[]
            {
                new ContractValidationRule(),
            };

            var results = RuleExecute.Execute(this, rules);

            if (Properties != null)
            {
                foreach (var Property in Properties)
                    results.Concate(Property.Valid());
            }

            if (ValidationArrayObjects != null)
            {
                foreach (var arrayObject in ValidationArrayObjects)
                    results.Concate(arrayObject.Valid());
            }

            return results;
        }
    }
}
