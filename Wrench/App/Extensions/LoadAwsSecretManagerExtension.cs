using App.Settings;

namespace App.Extensions
{
    public static class LoadAwsSecretManagerExtension
    {
        public static void AddSecretManager(this WebApplicationBuilder builder)
        {
            try
            {
                var appConfig = ApplicationSetting.Current;
                var configuration = builder.Configuration;
                var environment = builder.Environment;
                configuration.AddEnvironmentVariables();

                if (!environment.IsDevelopment()
                && !environment.IsEnvironment("Docker")
                && !environment.IsEnvironment("Test")
                && appConfig.Startup.AwsSecretEnable)
                {
                    configuration.AddSecretsManager(configurator: options =>
                    {
                        options.SecretFilter = entry => entry.Name.StartsWith(appConfig.Startup.Prefix);
                        options.KeyGenerator = (secret, name) => name.Replace($"{appConfig.Startup.Prefix}__", "");
                    });
                }

                Console.WriteLine("AddSecretManager success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
