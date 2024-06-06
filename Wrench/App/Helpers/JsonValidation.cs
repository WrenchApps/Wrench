using App.Helpers.Validations;
using App.Settings.ContractValidations;
using App.Settings.ContractValidations.Types;
using System.Text.Json.Nodes;

namespace App.Helpers
{
    public class JsonValidation
    {
        private readonly JsonObject _jsonObject;
        private JsonObject _jsonCurrentValidation;
        private string _prefixPropertyName = "";

        public JsonValidation(JsonObject jsonObject)
        {
            _jsonObject = jsonObject;
        }

        public ResultValidation Validate(List<PropertyValidation> PropertyValidations)
        {
            _jsonCurrentValidation = _jsonObject;
            return JsonObjectValidate(PropertyValidations);
        }

        public ResultValidation Validate(List<PropertyValidationArrayObject> validationArrayObjects)
        {
            var resultValidation = ResultValidation.Create();
            JsonObject jsonObjectCurrent = _jsonCurrentValidation;
            JsonNode mapFromJsonNode = null;

            foreach (var validationObject in validationArrayObjects)
            {
                var arrayPropertyNameSplited = validationObject.ArrayPropertyName.Split('.');
                foreach (var Property in arrayPropertyNameSplited)
                {
                    if (jsonObjectCurrent.TryGetPropertyValue(Property, out mapFromJsonNode))
                    {
                        if (mapFromJsonNode.GetType() == typeof(JsonArray))
                        {
                            var jsonArray = mapFromJsonNode as JsonArray;
                            int index = 0;
                            foreach (var jsonObject in jsonArray)
                            {
                                if (jsonObject.GetType() == typeof(JsonObject))
                                {
                                    _jsonCurrentValidation = jsonObject as JsonObject;
                                    _prefixPropertyName = $"{validationObject.ArrayPropertyName}[{index}].";
                                    var result = JsonObjectValidate(validationObject.Properties);
                                    resultValidation.Concate(result);
                                }

                                index++;
                            }
                        }
                    }
                    else
                        break;
                }
            }

            return resultValidation;
        }

        private ResultValidation JsonObjectValidate(List<PropertyValidation> PropertyValidations)
        {
            var resultValidation = ResultValidation.Create();
            var groupByProperty = PropertyValidations.GroupBy(p => p.PropertyName);

            foreach (var keyValue in groupByProperty)
            {
                var propertyValidation = PropertyValidate(keyValue);
                resultValidation.Append(propertyValidation);
            }

            return resultValidation;
        }

        private IEnumerable<ResultPropertyValidation> PropertyValidate(IGrouping<string, PropertyValidation> groupPropertyValidations)
        {
            var PropertyValidateSplitted = groupPropertyValidations.Key.Split('.');

            JsonNode mapFromJsonNode = null;
            JsonObject jsonObjectCurrent = _jsonCurrentValidation;
            var propertyName = $"{_prefixPropertyName}{groupPropertyValidations.Key}";
            var PropertyValidations = groupPropertyValidations.ToArray();

            foreach (var Property in PropertyValidateSplitted)
            {
                if (jsonObjectCurrent.TryGetPropertyValue(Property, out mapFromJsonNode))
                {
                    if (mapFromJsonNode.GetType() == typeof(JsonObject))
                        jsonObjectCurrent = mapFromJsonNode as JsonObject;
                }
                else
                    break;
            }

            var jsonValue = mapFromJsonNode?.AsValue();
            foreach (var PropertyValidation in PropertyValidations)
            {
                if (PropertyValidation.ValueType == PropertyValueType.String)
                    yield return StringPropertyValidation(propertyName, jsonValue, PropertyValidation);
                else if (PropertyValidation.ValueType == PropertyValueType.Int)
                    yield return IntPropertyValidation(propertyName, jsonValue, PropertyValidation);
                else if (PropertyValidation.ValueType == PropertyValueType.Float)
                    yield return FloatPropertyValidation(propertyName, jsonValue, PropertyValidation);
            }
        }

        private ResultPropertyValidation StringPropertyValidation(string propertyName, JsonValue jsonValue, PropertyValidation PropertyValidation)
        {
            var result = ResultPropertyValidation.Create(propertyName);
            if (jsonValue?.TryGetValue<string>(out var value) == true)
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.Required)
                {
                    if (string.IsNullOrEmpty(value))
                        result.SetError(PropertyValidation);
                }
                else if (PropertyValidation.ValidationType == PropertyValidationType.LessThan)
                {
                    if (value?.Count() >= PropertyValidation.Length)
                        result.SetError(PropertyValidation);
                }
                else if (PropertyValidation.ValidationType == PropertyValidationType.BiggerThan)
                {
                    if (value?.Count() <= PropertyValidation.Length)
                        result.SetError(PropertyValidation);
                }
            }
            else if (jsonValue != null)
                result.SetError(PropertyValidation);
            else
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.Required)
                {
                    result.SetError(PropertyValidation);
                }
            }

            return result;
        }

        private ResultPropertyValidation IntPropertyValidation(string propertyName, JsonValue jsonValue, PropertyValidation PropertyValidation)
        {
            var result = ResultPropertyValidation.Create(propertyName);
            if (jsonValue?.TryGetValue<int>(out var value) == true)
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.LessThan)
                {
                    if (value >= PropertyValidation.Value)
                        result.SetError(PropertyValidation);
                }
                else if (PropertyValidation.ValidationType == PropertyValidationType.BiggerThan)
                {
                    if (value <= PropertyValidation.Value)
                        result.SetError(PropertyValidation);
                }
            }
            else if (jsonValue != null)
                result.SetError(PropertyValidation);
            else
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.Required)
                {
                    result.SetError(PropertyValidation);
                }
            }

            return result;
        }

        private ResultPropertyValidation FloatPropertyValidation(string propertyName, JsonValue jsonValue, PropertyValidation PropertyValidation)
        {
            var result = ResultPropertyValidation.Create(propertyName);
            if (jsonValue?.TryGetValue<float>(out var value) == true)
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.LessThan)
                {
                    if (value >= PropertyValidation.Value)
                        result.SetError(PropertyValidation);
                }
                else if (PropertyValidation.ValidationType == PropertyValidationType.BiggerThan)
                {
                    if (value <= PropertyValidation.Value)
                        result.SetError(PropertyValidation);
                }
            }
            else if (jsonValue != null)
                result.SetError(PropertyValidation);
            else
            {
                if (PropertyValidation.ValidationType == PropertyValidationType.Required)
                {
                    result.SetError(PropertyValidation);
                }
            }

            return result;
        }
    }
}
