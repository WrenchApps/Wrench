using App.Constants;
using App.Contexts;
using App.Handlers.Http;
using App.HttpRequests;
using App.Settings;
using App.Validations;
using YamlDotNet.Serialization.NamingConventions;

namespace App.Extensions
{
    public static class LoadApplicationSettingsExtension
    {
        public static void AddLoadApplicationSettings(this WebApplicationBuilder builder)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                       .WithNamingConvention(CamelCaseNamingConvention.Instance)
                       .Build();


            var pathConfigFile = Environment.GetEnvironmentVariable("CONFIG_APP_PATH") ?? "configApp.yaml";
            var applicationSettings = deserializer.Deserialize<ApplicationSetting>(File.ReadAllText(pathConfigFile));

            LoadMapVariables();

            ApplicationSettingValidate(applicationSettings);
            Console.WriteLine($"AddConfigFile File {pathConfigFile} loaded and validated");

            builder.Services.AddScoped<StepladderHttpContext>();

            builder.Services.AddScoped<HttpFirstHandler>();
            builder.Services.AddScoped<HttpClientRequestHandler>();
            builder.Services.AddScoped<HttpWriteResponseHandler>();
            builder.Services.AddScoped<HttpWriteResponseMockHandler>();
            builder.Services.AddScoped<HttpResponseContractMapHandler>();
            builder.Services.AddScoped<HttpRequestContractValidationHandler>();
            builder.Services.AddScoped<HttpRequestStrategyCacheHandler>();
            builder.Services.AddScoped<HttpRequestStrategyHttpIdempotencyHandler>();


            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<HttpRequestClient>();
        }

        private static void ApplicationSettingValidate(ApplicationSetting setting)
        {
            var validables = setting.GetValidables();
            var result = ValidateResult.Create();
            foreach (var validable in validables)
                result.Concate(validable.Valid());

            if (result.HasError)
            {
                throw new Exception(result.ToString());
            }
        }

        private static void LoadMapVariables()
        {
            var appSetting = ApplicationSetting.Current;
            var redisConnectionString = Environment.GetEnvironmentVariable(EnvVariablesConst.REDIS_CONNECTION_STRING);
            if (string.IsNullOrEmpty(redisConnectionString) == false)
                SetRedisConnectionString(redisConnectionString);


            if (appSetting.Startup.MapVariables != null)
            {
                foreach (var mapVar in appSetting.Startup.MapVariables)
                {
                    var envValue = Environment.GetEnvironmentVariable(mapVar.From);
                    if (mapVar.To == EnvVariablesConst.REDIS_CONNECTION_STRING)
                        SetRedisConnectionString(envValue);
                }
            }
        }


        private static void SetRedisConnectionString(string connection)
        {
            if (ApplicationSetting.Current.Connections?.Redis != null)
                ApplicationSetting.Current.Connections.Redis.ConnectionString = connection;
        }
    }
}
