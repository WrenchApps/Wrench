using App.Settings.ContractValidations;
using App.Settings.ContractValidations.Rules;
using App.Settings.ContractValidations.Types;

namespace Test.Settings.ContractValidations.Rules
{
    public class PropertyValidationTypeStringRuleTest
    {
        [Fact]
        public void WhenPropertyValidationNotInformedLengthAndValidationTypeIsBiggerThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.String, ValidationType = PropertyValidationType.BiggerThan, Length = 0 };
            var rule = new PropertyValidationTypeStringRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Length should be informed");
            Assert.True(contaings);
        }

        [Fact]
        public void WhenPropertyValidationNotInformedLengthAndValidationTypeIsLessThan_ShouldReturnError()
        {
            // arrange
            var setting = new PropertyValidation() { ValueType = PropertyValueType.String, ValidationType = PropertyValidationType.LessThan, Length = 0 };
            var rule = new PropertyValidationTypeStringRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contaings = result.Errors.Contains("PropertyValidation.Length should be informed");
            Assert.True(contaings);
        }
    }
}
