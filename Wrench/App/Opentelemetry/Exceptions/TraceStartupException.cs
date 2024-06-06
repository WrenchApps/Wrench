using System;

namespace Opentelemetry.Exceptions
{
    public class TraceStartupException : Exception
    {
        public TraceStartupException() 
            : base("Should be configure the AddOpenTelemetryTracing before.") { }
    }
}
