using App.Constants;
using App.Settings;
using App.Settings.HttpClients;
using App.Settings.HttpClients.Tokens;

namespace App.Extensions
{
    public static class LoadHttpClientAuthenticationExtension
    {
        public static async Task AddHttpClientAuthenticationAsync(this WebApplicationBuilder builder)
        {
            var appConfig = ApplicationSetting.Current;

            if (appConfig.Startup?.HttpClientAuthentication?.Count > 0)
            {
                foreach (var authentication in appConfig.Startup.HttpClientAuthentication)
                {
                    var authToken = await LoadHttpClientAuthenticationAsync(authentication);
                    if (authToken != null)
                        ConstValues.PutValue(authentication.Id, authToken);

                    Task.Run(async () => await ScheduleLoadClientCredentialTokenAsync(authentication, authToken));
                }
            }

            Console.WriteLine("Loaded httpClientAuthentication");
        }

        private static async Task<AuthToken> LoadHttpClientAuthenticationAsync(HttpClientAuthentication authentication)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (httpMessage, certicate, x509Chain, sslPolicyErrors) => true
                };

                using (var httpClient = new HttpClient(handler))
                {
                    var headers = new Dictionary<string, string>()
                        {
                            { "client_id", authentication.ClientId },
                            { "client_secret", authentication.ClientSecret },
                            { "grant_type", "client_credentials" }
                        };

                    var content = new FormUrlEncodedContent(headers);
                    var response = httpClient.PostAsync(authentication.EndpointAuth, content).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();
                        authToken.AuthenticationId = authentication.Id;

                        return authToken;
                    }
                    else
                        throw new Exception($"Error when try load autenticate {authentication.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static async Task ScheduleLoadClientCredentialTokenAsync(HttpClientAuthentication authentication, AuthToken currentAuthToken)
        {
            while (true)
            {
                var secondsToReload = currentAuthToken.ExpiresIn - 180;
                await Task.Delay(secondsToReload * 1000);
                currentAuthToken = await LoadHttpClientAuthenticationAsync(authentication);
            }
        }
    }
}
