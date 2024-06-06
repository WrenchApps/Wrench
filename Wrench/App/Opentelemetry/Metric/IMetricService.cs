using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Opentelemetry.Metric
{
    public interface IMetricService
    {
        Meter GetCurrentMeter();
        KeyValuePair<string, object?> CreateCustomTag(string name, object? value);
        void CounterAdd<T>(string meterName, T value, params KeyValuePair<string, object?>[] tags) where T : struct;
        void HistogramRecord<T>(string meterName, T value, params KeyValuePair<string, object?>[] tags) where T : struct;
    }
}