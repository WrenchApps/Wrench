using App.Settings.ContractMap;
using App.Settings.ContractMap.Rules;

namespace Test.Settings.ContractMap.Rules
{
    public class ContractMapArrayRuleTest
    {
        [Fact]
        public void WhenContractMapArrayHasEmptyOrNullArrayMapFromTo_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapArray();
            var rule = new ContractMapArrayRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapArray.ArrayMapFromTo is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapArrayHasAnyMapFromToWithSpace_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapArray { MapFromTo = new List<string> { "street :rua" } };
            var rule = new ContractMapArrayRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapArray.MapFromTo not accept space");
            Assert.True(contains);
        }

        [Fact]
        public void WhenContractMapArrayHasAnyMapFromToWithDot_ShouldReturnError()
        {
            // arrange
            var setting = new ContractMapArray { MapFromTo = new List<string> { "address.street:rua" } };
            var rule = new ContractMapArrayRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractMapArray.MapFromTo not accept '.'");
            Assert.True(contains);
        }
    }
}
