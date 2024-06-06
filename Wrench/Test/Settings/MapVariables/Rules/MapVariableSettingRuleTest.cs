using App.Settings.MapVariables;
using App.Settings.MapVariables.Rules;

namespace Test.Settings.MapVariables.Rules
{
    public class MapVariableSettingRuleTest
    {
        [Fact]
        public void WhenMapVariableSettingHasEmptyFrom_ShouldReturnError()
        {
            // arrange
            var setting = new MapVariableSetting();
            var rule = new MapVariableSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("MapVariable.From is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenMapVariableSettingHasEmptyTo_ShouldReturnError()
        {
            // arrange
            var setting = new MapVariableSetting();
            var rule = new MapVariableSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("MapVariable.To is required");
            Assert.True(contains);
        }
    }
}
