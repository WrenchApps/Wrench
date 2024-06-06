using App.Settings;
using App.Settings.Strategies;
using App.Settings.Strategies.Rules;

namespace Test.Settings.Strategies.Rules
{
    public class StrategiesSettingRuleTest
    {
        [Fact]
        public void WhenStategyCacheIdIsDuplicated_ShouldReturnError()
        {
            // arrange
            var setting = new StrategiesSetting
            {
                Caches = new List<CacheSetting>
                {
                    new CacheSetting { Id = "duplicated_1" },
                    new CacheSetting { Id = "duplicated_1" }
                }
            };
            var rule = new StrategiesSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Strategies.Caches.Id duplicated_1 is duplicated");
            Assert.True(contains);
        }


        [Fact]
        public void WhenStategyHttpIdempotenciesIdIsDuplicated_ShouldReturnError()
        {
            // arrange
            var setting = new StrategiesSetting
            {
                HttpIdempotencies = new List<HttpIdempotencySetting> 
                {
                    new HttpIdempotencySetting { Id = "duplicated_1" },
                    new HttpIdempotencySetting { Id = "duplicated_1" }
                }
            };
            var rule = new StrategiesSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Strategies.HttpIdempotencies.Id duplicated_1 is duplicated");
            Assert.True(contains);
        }
    }
}
