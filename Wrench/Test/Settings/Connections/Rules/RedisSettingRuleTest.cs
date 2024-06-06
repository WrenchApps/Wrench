using App.Settings.Connections;
using App.Settings.Connections.Rules;

namespace Test.Settings.Connections.Rules
{
    public class RedisSettingRuleTest
    {
        [Fact]
        public void WhenRedisConnectionStringHasEmptyOrNullValue_ShouldReturnError()
        {
            // arrange
            var setting = new RedisSetting();
            var rule = new RedisSettingRule();

            // act
            var result =  rule.Do(setting);

            // assert
            var contains = result.Errors.Contains("Redis.ConnectionString is required");
            Assert.True(contains);
        }
    }
}
