using App.Validations;

namespace App.Settings.HttpClients.ValidateRules
{
    public class HttpClientAuthenticationClientCredentialValidateRule : IRule<HttpClientAuthentication>
    {
        public ValidateResult Do(HttpClientAuthentication value)
        {
            var result = ValidateResult.Create();

            if(value.Type == Types.AuthenticationType.ClientCredential)
            {
                if (string.IsNullOrEmpty(value.ClientId))
                    result.AddError("HttpClientAuthentication.ClientId is required");

                if (string.IsNullOrEmpty(value.ClientSecret))
                    result.AddError("HttpClientAuthentication.ClientSecret is required");

                if(string.IsNullOrEmpty(value.EndpointAuth))
                    result.AddError("HttpClientAuthentication.EndpointAuth is required");
                else
                {
                    Uri uri;
                    Uri.TryCreate(value.EndpointAuth, UriKind.Absolute, out uri);
                    if(uri is null)
                        result.AddError("HttpClientAuthentication.EndpointAuth should a valid uri");
                }
            }

            return result;
        }
    }
}
