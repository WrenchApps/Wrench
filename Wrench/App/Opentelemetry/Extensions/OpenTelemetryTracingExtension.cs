using Opentelemetry.Configuration;
using Opentelemetry.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Opentelemetry.Extensions
{
    public static class OpenTelemetryTracingExtension
    {
        internal static ActivitySource ACTIVITY_SOURCE;

        public static TracerProviderBuilder AddOpenTelemetryTracing(this IServiceCollection services, OpenTelemetryConfig config)
        {
            ACTIVITY_SOURCE = new ActivitySource(config.ServiceName, config.ServiceVersion);
            services.AddSingleton(ACTIVITY_SOURCE);
            services.AddSingleton<ITraceService, TraceService>();

            TracerProviderBuilder tracerBuilder = default;
            var protocol = config.IsGrpc ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
            var endpoint = protocol == OtlpExportProtocol.Grpc ? new Uri(config.Endpoint) : new Uri($"{config.Endpoint}/v1/traces");

            services.AddOpenTelemetryTracing(builder =>
            {
                tracerBuilder = builder;
                builder
                     .AddOtlpExporter(opt =>
                     {
                         opt.Endpoint = endpoint;
                         opt.Protocol = protocol;
                         opt.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
                     })
                    .AddSource(config.ServiceName)
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: config.ServiceName, serviceVersion: config.ServiceVersion))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation(o =>
                    {
                        o.Filter = context =>
                        {
                            return context.Request.Path.Value.ToLower() != "/hc" &&
                                   context.Request.Path.Value.ToLower() != "/liveness";
                        };
                    });

                if (config.EnableConsoleExporter)
                    builder.AddConsoleExporter();
            });

            return tracerBuilder;
        }
    }
}
