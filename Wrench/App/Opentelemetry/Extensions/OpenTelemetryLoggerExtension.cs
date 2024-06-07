using System;
using System.Net.Http;
using Opentelemetry.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Opentelemetry.Extensions
{
    public static class OpenTelemetryLoggerExtension
    {
        public static ILoggingBuilder AddOpenTelemetryLogging(this ILoggingBuilder builder, OpenTelemetryConfig config)
        {
            var protocol = config.IsGrpc ? OtlpExportProtocol.Grpc : OtlpExportProtocol.HttpProtobuf;
            var endpoint = protocol == OtlpExportProtocol.Grpc ? new Uri(config.Endpoint) : new Uri($"{config.Endpoint}/v1/logs");

            builder.AddOpenTelemetry((OpenTelemetryLoggerOptions loggingbuilder) =>
            {
                loggingbuilder.AddOtlpExporter(opt =>
                {
                    opt.Endpoint = endpoint;
                    opt.Protocol = protocol;
                    opt.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;

                }).SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: config.ServiceName, serviceVersion: config.ServiceVersion));

                if (config.EnableConsoleExporter)
                    loggingbuilder.AddConsoleExporter();
            });

            return builder;
        }
    }
}
