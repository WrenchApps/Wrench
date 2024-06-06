using App.Settings;
using Opentelemetry.Configuration;
using Opentelemetry.Extensions;
using OpenTelemetry.Trace;

namespace App.Extensions
{
    public static class LoadTelemetricExtension
    {
        public static void AddTelemetric(this WebApplicationBuilder builder)
        {
            var appSetting = ApplicationSetting.Current;

            if (appSetting.Startup.EnableTelemetry)
            {
                var otelConfig = new OpenTelemetryConfig
                {
                    ServiceName = appSetting.Startup.ServiceName,
                    ServiceVersion = appSetting.Startup.ServiceVersion,
                    Endpoint = appSetting.Startup.OtelEndpoint,
                    IsGrpc = false,
                    EnableConsoleExporter = true
                };

                builder.Logging.AddOpenTelemetryLogging(otelConfig);
                builder.Services.AddOpenTelemetryMetrics(otelConfig);

                var tracing = builder.Services.AddOpenTelemetryTracing(otelConfig);
                tracing.AddRedisInstrumentation();
            }
        }
    }
}
