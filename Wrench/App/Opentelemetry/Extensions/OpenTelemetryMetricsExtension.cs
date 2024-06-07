using Opentelemetry.Configuration;
using Opentelemetry.Exceptions;
using Opentelemetry.Http;
using Opentelemetry.Metric;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics.Metrics;


namespace Opentelemetry.Extensions
{
    public static class OpenTelemetryMetricsExtension
    {
        internal static Meter METER;
        internal static Histogram<int> HTTP_REQUEST_ELAPSED_TIME;
        private static Counter<int> HEART_BEAT_COUNTER;

        public static MeterProviderBuilder AddOpenTelemetryMetrics(this IServiceCollection services, OpenTelemetryConfig config)
        {
            config.Valid();
            if (config.IgnoreRoutes?.Count > 0)
                HttpRequestMetricsMiddleware.IgnoreRoutes.AddRange(config.IgnoreRoutes.Select(x => x.ToLower()));

            MeterProviderBuilder meterBuilder = default;
            METER = new Meter(config.ServiceName, config.ServiceVersion);

            HEART_BEAT_COUNTER = METER.CreateCounter<int>("heart_beat");
            METER.CreateObservableGauge("process_memory_usage", () => { return System.Diagnostics.Process.GetCurrentProcess().WorkingSet64; });
            METER.CreateObservableGauge("process_memory_virtual", () => { return System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64; });

            services.AddSingleton(METER);
            services.AddSingleton<IMetricService, MetricService>();

            var protocol = config.IsGrpc ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
            var endpoint = protocol == OtlpExportProtocol.Grpc ? new Uri(config.Endpoint) : new Uri($"{config.Endpoint}/v1/metrics");

            services.AddOpenTelemetryMetrics(builder =>
            {
                meterBuilder = builder;
                builder.AddMeter(config.ServiceName, config.ServiceVersion);
                builder.SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: config.ServiceName, serviceVersion: config.ServiceVersion));

                builder.AddOtlpExporter((OtlpExporterOptions opt, MetricReaderOptions metricOpt) =>
                {
                    opt.Endpoint = endpoint;
                    opt.Protocol = protocol;
                    opt.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;

                    if (config.MetricsExportIntervalSeconds.HasValue)
                    {
                        metricOpt.PeriodicExportingMetricReaderOptions = new PeriodicExportingMetricReaderOptions
                        {
                            ExportIntervalMilliseconds = (config.MetricsExportIntervalSeconds.Value * 1000)
                        };
                    }
                })
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation();

                if (config.EnableConsoleExporter)
                    builder.AddConsoleExporter();
            });

            Task.Run(HeartBeatAsync);
            return meterBuilder;
        }

        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            if (METER is null)
                throw new MetricStartupException();

            HTTP_REQUEST_ELAPSED_TIME = METER.CreateHistogram<int>("request_elapsed_time");
            return builder.UseMiddleware<HttpRequestMetricsMiddleware>();
        }

        private static async Task HeartBeatAsync()
        {
            while (true)
            {
                HEART_BEAT_COUNTER.Add(1);
                await Task.Delay(10000);
            }
        }
    }
}
