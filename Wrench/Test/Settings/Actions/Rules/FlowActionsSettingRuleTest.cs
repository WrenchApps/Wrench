using App.Settings;
using App.Settings.Actions;
using App.Settings.Actions.Rules;

namespace Test.Settings.Actions.Rules
{
    public class FlowActionsSettingRuleTest
    {
        [Fact]
        public void WhenFlowActionsSettingHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var setting = new FlowActionsSetting { Id = null };
            var rule = new FlowActionsSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("FlowActionsSetting.Id is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenFlowActionsSettingHasActionsIdNotConfigured_ShouldReturnError()
        {
            // arrange
            ApplicationSetting.Current.Actions = new List<ActionSetting>() { new ActionSetting { Id = "request_goggle" } };
            var setting = new FlowActionsSetting { ActionsId = new List<string> { "request_goggle", "not_configured" } };
            var rule = new FlowActionsSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("FlowActionsSetting.ActionsId not_configured should be configured");
            Assert.True(contains);
        }

        [Fact]
        public void WhenFlowActionsSettingHasActionsIdConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.Actions = new List<ActionSetting>(){ new ActionSetting { Id = "request_goggle" } };
            var setting = new FlowActionsSetting { ActionsId = new List<string> { "request_goggle" } };
            var rule = new FlowActionsSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("FlowActionsSetting.ActionsId request_goggle should be configured");
            Assert.False(contains);
        }

        [Fact]
        public void WhenFlowActionsSettingHasEmptyOrNullActionsId_ShouldReturnError()
        {
            // arrange
            var setting = new FlowActionsSetting { ActionsId = new List<string>() };
            var rule = new FlowActionsSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("FlowActionsSetting.ActionsId is required");
            Assert.True(contains);
        }
    }
}
