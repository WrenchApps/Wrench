using App.Settings;
using App.Settings.Connections;
using App.Settings.Strategies;
using App.Settings.Strategies.Rules;
using App.Settings.Strategies.Types;

namespace Test.Settings.Strategies.Rules
{
    public class HttpIdempotencySettingRuleTest
    {
        [Fact]
        public void WhenHttpIdempotencyHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var setting = new HttpIdempotencySetting();
            var rule = new HttpIdempotencySettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("HttpIdempotency.Id is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenHttpIdempotencyHasNoneProviderType_ShouldReturnError()
        {
            // arrange
            var setting = new HttpIdempotencySetting { ProviderType = StrategyProviderType.None };
            var rule = new HttpIdempotencySettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("HttpIdempotency.ProviderType is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenConfigureHttpIdempotencyRedisWioutConfigureConnectionRedis_ShouldReturnError()
        {
            // arrange
            ApplicationSetting.Current.Connections = new ConnectionSetting() { Redis = null };
            var setting = new HttpIdempotencySetting { ProviderType = StrategyProviderType.Redis };
            var rule = new HttpIdempotencySettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Configure connection redis before use cache redis");
            Assert.True(contains);
        }

        [Fact]
        public void WhenConfigureHttpIdempotencyTtlWithInvalidValue_ShouldReturnError()
        {
            // arrange
            var setting = new HttpIdempotencySetting { Ttl = 9 };
            var rule = new HttpIdempotencySettingRule();

            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("HttpIdempotency.Ttl should be bigger than 9 seconds");
            Assert.True(contains);
        }
    }
}
