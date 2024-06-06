using App.Settings.Actions;
using App.Settings.Actions.Rules;

namespace Test.Settings.Actions.Rules
{
    public class ActionHeaderMapRuleTest
    {
        [Fact]
        public void WhenActionHeaderMapHasNullOrEmptyMapFromTo_ShouldReturnError()
        {
            // arrange
            var setting = new ActionHeaderMap();
            var rule = new ActionHeaderMapRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ActionHeaderMap.MapFromTo is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenActionHeaderMapMapFromToHasSpace_ShouldReturnError()
        {
            // arrange
            var setting = new ActionHeaderMap { MapFromTo = "test: test" };
            var rule = new ActionHeaderMapRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ActionHeaderMap.MapFromTo not accept space");
            Assert.True(contains);
        }

        [Fact]
        public void WhenActionHeaderMapMapFromToNotHasSplittedValid_ShouldReturnError()
        {
            // arrange
            var setting = new ActionHeaderMap { MapFromTo = "testtest" };
            var rule = new ActionHeaderMapRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ActionHeaderMap.MapFromTo is invalid the mapFromTo should splitted with one ':'");
            Assert.True(contains);
        }

        [Fact]
        public void WhenActionHeaderMapHasNullOrEmptyFromType_ShouldReturnError()
        {
            // arrange
            var setting = new ActionHeaderMap();
            var rule = new ActionHeaderMapRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("ActionHeaderMap.FromType is required");
            Assert.True(contains);
        }
    }
}
