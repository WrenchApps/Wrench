using App.Settings;
using App.Settings.Connections;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace App.Extensions
{
    public static class LoadConnectionsExtension
    {
        public static void AddConnections(this WebApplicationBuilder builder)
        {
            var appConfig = ApplicationSetting.Current;
            if (appConfig.Connections != null)
            {
                if (appConfig.Connections.Redis != null)
                {
                    RedisConnection(builder, appConfig.Connections.Redis);
                }
            }
        }

        private static void RedisConnection(WebApplicationBuilder builder, RedisSetting redisSetting)
        {
            var multiplexer = ConnectionMultiplexer.Connect(redisSetting.ConnectionString);

            if (multiplexer.IsConnected == false)
                throw new Exception($"Redis error when try connection in {redisSetting.ConnectionString}");

            var database = multiplexer.GetDatabase();

            builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            builder.Services.AddSingleton(database);
            ConfigureRedLockFactory(builder, multiplexer);
        }

        private static void ConfigureRedLockFactory(WebApplicationBuilder builder, params ConnectionMultiplexer[] multiplexers)
        {
            var appConfig = ApplicationSetting.Current;
            if (appConfig.Strategies?.HttpIdempotencies?.Count > 0)
            {
                var listMultiplexers = new List<RedLockMultiplexer>();

                foreach (var multiplexer in multiplexers)
                    listMultiplexers.Add(new RedLockMultiplexer(multiplexer));


                var redLockFactory = RedLockFactory.Create(listMultiplexers);
                builder.Services.AddSingleton(redLockFactory);
            }
        }
    }
}
