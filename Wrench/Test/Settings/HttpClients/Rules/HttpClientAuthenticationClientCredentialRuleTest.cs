using App.Settings.HttpClients;
using App.Settings.HttpClients.ValidateRules;

namespace Test.Settings.HttpClients.ValidateRules
{
    public class HttpClientAuthenticationClientCredentialRuleTest
    {
        [Fact]
        public void WhenClientIdIsNullOrEmpty_ShouldReturnError()
        {
            // arrange 
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationClientCredentialValidateRule();

            // act 

            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.ClientId is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenClientSecretIsNullOrEmpty_ShouldReturnError()
        {
            // arrange 
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationClientCredentialValidateRule();

            // act 
            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.ClientSecret is required");
            Assert.True(contains);
        }


        [Fact]
        public void WhenEndpointAuthIsNullOrEmpty_ShouldReturnError()
        {
            // arrange 
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationClientCredentialValidateRule();

            // act 
            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.EndpointAuth is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenEndpointAuthIsNotValid_ShouldReturnError()
        {
            // arrange 
            var httpClientAuthentication = new HttpClientAuthentication
            {
                EndpointAuth = "http://ts@tes@com"
            };
            var rule = new HttpClientAuthenticationClientCredentialValidateRule();

            // act 
            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.EndpointAuth should a valid uri");
            Assert.True(contains);
        }

        [Fact]
        public void WhenTokenUriIsValid_ShouldNotReturnError()
        {
            // arrange 
            var httpClientAuthentication = new HttpClientAuthentication
            {
                EndpointAuth = "http://pix.com/api/test"
            };
            var rule = new HttpClientAuthenticationClientCredentialValidateRule();

            // act 
            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.TokenUri should a valid uri");
            Assert.False(contains);
        }
    }
}
