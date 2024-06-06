using App.Settings;
using App.Settings.Connections;
using App.Settings.Strategies;
using App.Settings.Strategies.Rules;
using App.Settings.Strategies.Types;

namespace Test.Settings.Strategies.Rules
{
    public class CacheSettingRuleTest
    {
        [Fact]
        public void WhenCacheHasEmptyOrNullId_ShouldReturnError()
        {
            // arrange
            var setting = new CacheSetting();
            var rule = new CacheSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Cache.Id is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenCacheHasNoneProviderType_ShouldReturnError()
        {
            // arrange
            var setting = new CacheSetting { ProviderType = StrategyProviderType.None };
            var rule = new CacheSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Cache.ProviderType is required");
            Assert.True(contains);
        }

        [Fact]
        public void WhenConfigureCacheRedisWioutConfigureConnectionRedis_ShouldReturnError()
        {
            // arrange
            ApplicationSetting.Current.Connections = new ConnectionSetting() { Redis = null };
            var setting = new CacheSetting { ProviderType = StrategyProviderType.Redis };
            var rule = new CacheSettingRule();

            // act
            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Configure connection redis before use cache redis");
            Assert.True(contains);
        }

        [Fact]
        public void WhenConfigureCacheTtlWithInvalidValue_ShouldReturnError()
        {
            // arrange
            var setting = new CacheSetting { Ttl = 9 };
            var rule = new CacheSettingRule();

            var result = rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Cache.Ttl should be bigger than 9 seconds");
            Assert.True(contains);
        }
    }
}
