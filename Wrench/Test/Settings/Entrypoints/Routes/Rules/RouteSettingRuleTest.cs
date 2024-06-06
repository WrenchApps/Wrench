using App.Settings;
using App.Settings.Actions;
using App.Settings.Entrypoints.Routes;
using App.Settings.Entrypoints.Routes.Rules;

namespace Test.Settings.Entrypoints.Routes.Rules
{
    public class RouteSettingRuleTest
    {
        [Fact]
        public void WhenRouteSettingMethodIsNullOrEmpty_ShouldReturnError()
        {
            // arrange
            var routeSetting = new RouteSetting();
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var constains = result.Errors.Contains("Route.Method is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenRouteSettingRouteIsNullOrEmpty_ShouldReturnError()
        {
            // arrange
            var routeSetting = new RouteSetting();
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var constains = result.Errors.Contains("Route.Route is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenRouteSettingFlowActionIdIsNullOrEmptyAndResponseMockIsNull_ShouldReturnError()
        {
            // arrange
            var routeSetting = new RouteSetting { FlowActionId = null, ResponseMock = null };
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var constains = result.Errors.Contains("Route.FlowActionId is required");
            Assert.True(constains);
        }

        [Fact]
        public void WhenRouteSettingFlowActionIdIsNullOrEmptyAndResponseMockIsNotNull_ShouldReturnSuccess()
        {
            // arrange
            var routeSetting = new RouteSetting { FlowActionId = null, ResponseMock = new ResponseMock() };
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var constains = result.Errors.Contains("Route.FlowActionId is required");
            Assert.False(constains);
        }

        [Fact]
        public void WhenRouteSettingFlowActionIdNotConfigured_ShouldReturnError()
        {
            // arrange
            ApplicationSetting.Current.FlowActions = new List<FlowActionsSetting>() { new FlowActionsSetting { Id = "create_account" } };
            var routeSetting = new RouteSetting { FlowActionId = "create_account_not_configured" };
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var contains = result.Errors.Contains("Route.FlowActionId create_account_not_configured should be configured");
            Assert.True(contains);
        }

        [Fact]
        public void WhenRouteSettingFlowActionIdIsConfigured_ShouldReturnSuccess()
        {
            // arrange
            ApplicationSetting.Current.FlowActions = new List<FlowActionsSetting>() { new FlowActionsSetting { Id = "create_account" } };
            var routeSetting = new RouteSetting { FlowActionId = "create_account" };
            var rule = new RouteSettingRule();

            // act
            var result = rule.Do(routeSetting);

            // assert
            var contains = result.Errors.Contains("Route.FlowActionId create_account should be configured");
            Assert.False(contains);
        }

    }
}
