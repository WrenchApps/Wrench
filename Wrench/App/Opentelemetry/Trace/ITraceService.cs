using System.Diagnostics;

namespace Opentelemetry.Trace
{
    public interface ITraceService
    {
        ActivitySource GetActivitySource();
        Activity StartActivity(string name = "", ActivityKind kind = ActivityKind.Internal, string activityId = null);
    }
}