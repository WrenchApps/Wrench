using App.Settings.ContractValidations;
using App.Settings.ContractValidations.Rules;

namespace Test.Settings.ContractValidations.Rules
{
    public class ContractValidationRuleTest
    {
        [Fact]
        public void WhenContractValidationHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var setting = new ContractValidation();
            var rule = new ContractValidationRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ContractValidation.Id is required");
            Assert.True(contains);
        }
    }
}
