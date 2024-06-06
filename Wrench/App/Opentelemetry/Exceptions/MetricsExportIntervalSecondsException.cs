using System;

namespace Opentelemetry.Exceptions
{
    public class MetricsExportIntervalSecondsException : Exception
    {
        public MetricsExportIntervalSecondsException()
           : base($"The value to interval should be greater than {ConstValues.MIN_METRICS_INTERVAL_SECONDS}.") { }
    }
}
