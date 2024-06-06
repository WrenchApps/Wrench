using App.Settings.ApiSecurets;
using App.Settings.ApiSecurets.Types;
using App.Settings.ApiSecurets.ValidateRules;

namespace Test.Settings.ApiSecurets.ValidateRules
{
    public class ApiSecuritySettingTypeBasicRuleTest
    {
        [Fact]
        public void WhenApiSecuretTypeBasicHasEmptyOrNullUser_ShouldReturnError()
        {
            // arrange
            var apiSecuret = new ApiSecuritySetting();
            var rule = new ApiSecuritySettingTypeBasicRule();

            // act
            var result = rule.Do(apiSecuret);

            // assert
            var constains = result.Errors.Contains("ApiSecurity.User is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenApiSecuretTypeBasicHasEmptyOrNullPassword_ShouldReturnError()
        {
            // arrange
            var apiSecuret = new ApiSecuritySetting();
            var rule = new ApiSecuritySettingTypeBasicRule();

            // act
            var result = rule.Do(apiSecuret);

            // assert
            var constains = result.Errors.Contains("ApiSecurity.Password is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenApiSecuretTypeBasicHasTypeNone_ShouldReturnError()
        {
            // arrange
            var apiSecuret = new ApiSecuritySetting { Type = ApiSecurityType.None };
            var rule = new ApiSecuritySettingTypeBasicRule();

            // act
            var result = rule.Do(apiSecuret);

            // assert
            var constains = result.Errors.Contains("ApiSecurity.Type is required");
            Assert.True(constains);
        }
    }
}
