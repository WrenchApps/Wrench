using System;

namespace Opentelemetry.Exceptions
{
    public class MetricStartupException : Exception
    {
        public MetricStartupException() 
            : base("Should be configure the AddOpenTelemetryMetrics before.") { }
    }
}
