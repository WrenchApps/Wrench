using App.Settings.ContractValidations;
using App.Settings.ContractValidations.Rules;
using App.Settings.ContractValidations.Types;

namespace Test.Settings.ContractValidations.Rules
{
    public class PropertyValidationRuleTest
    {
        [Fact]
        public void WhenPropertyValidationHasNullOrEmptyPropertyName_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation();
            var rule = new PropertyValidationRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.PropertyName is required");
            Assert.True(contaings);
        }

        [Fact]
        public void WhenPropertyValidationHasSpaceProperty_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { PropertyName = "customer. name" };
            var rule = new PropertyValidationRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.PropertyName not accept space");
            Assert.True(contaings);
        }


        [Fact]
        public void WhenPropertyValidationNotInformedType_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValidationType = PropertyValidationType.None };
            var rule = new PropertyValidationRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.ValidationType is required");
            Assert.True(contaings);
        }
    }
}
