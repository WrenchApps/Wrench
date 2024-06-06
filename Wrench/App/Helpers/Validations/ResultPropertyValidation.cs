using App.Settings.ContractValidations;
using App.Settings.ContractValidations.Types;
using System.Text.Json.Serialization;

namespace App.Helpers.Validations
{
    public class ResultPropertyValidation
    {
        private static string PREFIX = "INVALID_PROPERTY_VALUE";

        [JsonPropertyName("key")]
        public string Key { get; private set; }

        [JsonPropertyName("propertyName")]
        public string PropertyName { get; private set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; private set; }


        [JsonIgnore]
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public void SetError(PropertyValidation propertyValidation)
        {
            if(propertyValidation.ValidationType == PropertyValidationType.Required)
            {
                Key = $"{PREFIX}_IS_REQUIRED";
                ErrorMessage = "Property value is required";
            }
            else if (propertyValidation.ValidationType == PropertyValidationType.BiggerThan)
            {
                Key = $"{PREFIX}_BIGGER_THAN";
                if(propertyValidation.ValueType == PropertyValueType.String)
                {
                    ErrorMessage = $"Property value should be bigger than {propertyValidation.Length} caracters";
                }
                else if (propertyValidation.ValueType == PropertyValueType.Int ||
                    propertyValidation.ValueType == PropertyValueType.Float)
                {
                    ErrorMessage = $"Property value should be bigger than {propertyValidation.Value}";
                }
            }
            else if (propertyValidation.ValidationType == PropertyValidationType.LessThan)
            {
                Key = $"{PREFIX}_LESS_THAN";
                if (propertyValidation.ValueType == PropertyValueType.String)
                {
                    ErrorMessage = $"Property value should be less than {propertyValidation.Length} caracters";
                }
                else if (propertyValidation.ValueType == PropertyValueType.Int ||
                    propertyValidation.ValueType == PropertyValueType.Float)
                {
                    ErrorMessage = $"Property value should be less than {propertyValidation.Value}";
                }
            }
        }

        public static ResultPropertyValidation Create(string propertyName)
        {
            var result = new ResultPropertyValidation { PropertyName = propertyName };
            return result;
        }
    }
}
