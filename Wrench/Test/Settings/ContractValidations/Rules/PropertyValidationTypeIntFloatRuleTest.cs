using App.Settings.ContractValidations.Rules;
using App.Settings.ContractValidations.Types;
using App.Settings.ContractValidations;

namespace Test.Settings.ContractValidations.Rules
{
    public class PropertyValidationTypeIntFloatRuleTest
    {
        [Fact]
        public void WhenPropertyValidationIntNotInformedValueAndValidationTypeIsBiggerThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.Int, ValidationType = PropertyValidationType.BiggerThan };
            var rule = new PropertyValidationTypeIntFloatRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Value should be informed");
            Assert.True(contaings);
        }

        [Fact]
        public void WhenPropertyValidationIntNotInformedValueAndValidationTypeIsLessThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.Int, ValidationType = PropertyValidationType.LessThan };
            var rule = new PropertyValidationTypeIntFloatRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Value should be informed");
            Assert.True(contaings);
        }


        [Fact]
        public void WhenPropertyValidationFloatNotInformedValueAndValidationTypeIsBiggerThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.Float, ValidationType = PropertyValidationType.BiggerThan };
            var rule = new PropertyValidationTypeIntFloatRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Value should be informed");
            Assert.True(contaings);
        }

        [Fact]
        public void WhenPropertyValidationFloatNotInformedValueAndValidationTypeIsLessThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.Float, ValidationType = PropertyValidationType.LessThan };
            var rule = new PropertyValidationTypeIntFloatRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Value should be informed");
            Assert.True(contaings);
        }
    }
}
