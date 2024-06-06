using System.Diagnostics;
using Opentelemetry.Exceptions;
using Opentelemetry.Extensions;

namespace Opentelemetry.Trace
{
    public sealed class TraceService : ITraceService
    {
        public ActivitySource GetActivitySource()
        {
            if (OpenTelemetryTracingExtension.ACTIVITY_SOURCE is null)
                throw new TraceStartupException();

            return OpenTelemetryTracingExtension.ACTIVITY_SOURCE;
        }

        public Activity StartActivity(string name = "", ActivityKind kind = ActivityKind.Internal, string activityId = null)
        {
            if (string.IsNullOrEmpty(activityId))
                return GetActivitySource().StartActivity(name, kind);
            else
            {
                var (parentTraceId, parentSpanId) = GetParents(activityId);
                if (parentTraceId is null || parentSpanId is null)
                    return StartActivity(name, kind);

                var parentContext = new ActivityContext(
                    ActivityTraceId.CreateFromString(parentTraceId),
                    ActivitySpanId.CreateFromString(parentSpanId),
                    ActivityTraceFlags.Recorded);

                return GetActivitySource().StartActivity(name, kind, parentContext);
            }
        }

        private (string parentTraceId, string parentSpanId) GetParents(string externalTraceId)
        {
            try
            {
                var traceValues = externalTraceId.Split('-');
                if (traceValues.Length > 0)
                {
                    return (traceValues[1], traceValues[2]);
                }

                return (null, null);
            }
            catch
            {
                return (null, null);
            }
        }
    }
}
