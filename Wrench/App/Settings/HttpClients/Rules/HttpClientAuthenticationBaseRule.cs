using App.Validations;

namespace App.Settings.HttpClients.ValidateRules
{
    public class HttpClientAuthenticationBaseRule : IRule<HttpClientAuthentication>
    {
        public ValidateResult Do(HttpClientAuthentication value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.Id))
                result.AddError("HttpClientAuthentication.Id is required");


            return result;
        }
    }
}
