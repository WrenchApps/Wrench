using System.Text.Json.Serialization;

namespace App.Helpers.Validations
{
    public class ResultValidation
    {
        private bool _success = true;
        public ResultValidation() { }

        [JsonPropertyName("errors")]
        public List<ResultPropertyValidation> PropertyValidation { get; private set; } = new List<ResultPropertyValidation>();

        [JsonIgnore]
        public bool Success => _success;

        public void Append(ResultPropertyValidation propertyValidation)
        {
            if (propertyValidation.Success)
                return;

            _success = false;
            PropertyValidation.Add(propertyValidation);
        }

        public void Concate(ResultValidation resultValidation)
            => Append(resultValidation.PropertyValidation.ToArray());

        public void Append(IEnumerable<ResultPropertyValidation> propertyValidations)
        {
            foreach (var propertyValidation in propertyValidations)
                Append(propertyValidation);
        }

        public static ResultValidation Create()
           => new ResultValidation();
    }
}
