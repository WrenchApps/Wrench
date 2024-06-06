using App.Settings.HttpClients;
using App.Settings.HttpClients.ValidateRules;

namespace Test.Settings.HttpClients.ValidateRules
{
    public class HttpClientAuthenticationBaseRuleTest
    {
        [Fact]
        public void WhenIdIsNullOrEmpty_ShouldReturnError()
        {
            // arrange
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationBaseRule();

            // act
            var result = rule.Do(httpClientAuthentication);

            // assert
            var contains = result.Errors.Contains("HttpClientAuthentication.Id is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenHttpClientAuthenticationHasError_ShouldReturnErrorTrue()
        {
            // arrange
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationBaseRule();

            // act
            var result = rule.Do(httpClientAuthentication);

            // assert
            Assert.True(result.HasError);
        }

        [Fact]
        public void WhenHttpClientAuthenticationHasError_ShouldReturnIsSuccessFalse()
        {
            // arrange
            var httpClientAuthentication = new HttpClientAuthentication();
            var rule = new HttpClientAuthenticationBaseRule();

            // act
            var result = rule.Do(httpClientAuthentication);

            // assert
            Assert.False(result.IsSuccess);
        }
    }
}
