using App.Settings.Actions;
using App.Settings.Actions.Rules;

namespace Test.Settings.Actions.Rules
{
    public class ActionRouteMapRuleTest
    {
        [Fact]
        public void WhenActionRouteMapRuleHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var config = new ActionRouteMap();
            var rule = new ActionRouteMapRule();

            // act
            var result = rule.Do(config);

            // assert
            var contains = result.Errors.Contains("ActionRouteMap.RouteKey is required");
        }

        [Fact]
        public void WhenActionRouteMapRuleHasEmptyOrNullFromType_ShouldReturnError()
        {
            // arrange
            var config = new ActionRouteMap();
            var rule = new ActionRouteMapRule();

            // act
            var result = rule.Do(config);

            // assert
            var contains = result.Errors.Contains("ActionRouteMap.FromType is required");
        }
    }
}
