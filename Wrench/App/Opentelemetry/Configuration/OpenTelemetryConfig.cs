using Opentelemetry.Exceptions;

namespace Opentelemetry.Configuration
{
    public class OpenTelemetryConfig
    {
        public string ServiceName { get; set; }
        public string ServiceVersion { get; set; }
        public string Endpoint { get; set; }
        public bool IsGrpc { get; set; }
        public bool EnableConsoleExporter { get; set; }
        public int? MetricsExportIntervalSeconds { get; set; }
        public List<string>? IgnoreRoutes { get; set; }

        public void Valid()
        {
            if (MetricsExportIntervalSeconds.HasValue && 
                MetricsExportIntervalSeconds.Value < ConstValues.MIN_METRICS_INTERVAL_SECONDS)
            {
                throw new MetricsExportIntervalSecondsException();
            }
        }
    }
}