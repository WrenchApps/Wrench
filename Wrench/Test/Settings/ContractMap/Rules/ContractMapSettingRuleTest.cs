using App.Settings.ContractMap;
using App.Settings.ContractMap.Rules;

namespace Test.Settings.ContractMap.Rules
{
    public class ContractMapSettingRuleTest
    {
        [Fact]
        public void WhenContractMapSettingHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapSetting { Id = null };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.Id is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapSettingHasNullMapFromTo_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapSetting { MapFromTo = null };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.MapFromTo is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapSettingHasEmptyMapFromTo_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapSetting { MapFromTo = new List<string>() };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.MapFromTo is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapSettingHasMapFromToWithMultipleSpliterCaracter_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapSetting { MapFromTo = new List<string>() { "customer.name:customer.fullName:customer.lastName" } };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.MapFromTo should separated with ':' ex 'customer.name:customer.fullName'");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapSettingHasMapFromToWithoutSpliterCaracter_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapSetting { MapFromTo = new List<string>() { "customer.name" } };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.MapFromTo should separated with ':' ex 'customer.name:customer.fullName'");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapSettingHasMapFromToWithSpliterCaracter_ShouldReturnSuccess()
        {
            // arrange
            var setting = new ContractMapSetting { MapFromTo = new List<string>() { "customer.name:customer.fullName" } };
            var rule = new ContractMapSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapSetting.MapFromTo should separated with ':' ex 'customer.name:customer.fullName'");
            Assert.False(contains);
        }
    }
}
