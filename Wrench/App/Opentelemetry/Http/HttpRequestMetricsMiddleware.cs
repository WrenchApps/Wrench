using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Opentelemetry.Extensions;
using Opentelemetry.Metric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Opentelemetry.Http
{
    internal class HttpRequestMetricsMiddleware
    {
        internal static List<string> IgnoreRoutes { get; private set; }
        private static List<string> IgnorePaths { get; set; }
        private readonly RequestDelegate _next;

        static HttpRequestMetricsMiddleware()
        {
            IgnoreRoutes = new List<string>();
            IgnorePaths = new List<string> { "/hc", "/liveness" };
        }

        public HttpRequestMetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
            var routeValue = GetRoute(context);

            var shouldIgnore = IgnoreRoutes.Contains(routeValue) || IgnorePaths.Contains(path);

            if (shouldIgnore)
            {
                await _next(context);
            }
            else
            {
                var listTags = new List<KeyValuePair<string, object?>>();
                var correlationId = GetCorrelationId(context);
                var companyKey = GetCompanyKey(context);

                listTags.Add(MetricService.CreateTag(ConstValues.KEY_HTTP_ROUTE, routeValue));
                listTags.Add(MetricService.CreateTag(ConstValues.KEY_COMPANY_KEY, companyKey));
                listTags.Add(MetricService.CreateTag(ConstValues.KEY_HTTP_METHOD, context.Request.Method));

                var activity = Activity.Current;
                if (activity != null)
                {
                    activity.SetTag(ConstValues.KEY_CORRELATION_ID, correlationId);
                    activity.SetTag(ConstValues.KEY_COMPANY_KEY, companyKey);
                }

                var watch = Stopwatch.StartNew();
                try
                {
                    await _next(context);
                    var statusCode = context.Response.StatusCode;
                    listTags.Add(MetricService.CreateTag(ConstValues.KEY_STATUS_CODE, statusCode));

                    if (statusCode >= 200 && statusCode <= 299)
                        listTags.Add(MetricService.CreateTag(ConstValues.KEY_STATUS_CODE_FAMILY, "200"));
                    else if (statusCode >= 400 && statusCode <= 499)
                        listTags.Add(MetricService.CreateTag(ConstValues.KEY_STATUS_CODE_FAMILY, "400"));
                }
                catch (Exception ex)
                {
                    var exception = ex.InnerException ?? ex;
                    listTags.Add(MetricService.CreateTag(ConstValues.KEY_STATUS_CODE_FAMILY, "500"));
                    listTags.Add(MetricService.CreateTag(ConstValues.EXCEPTION_FULL_NAME, exception.GetType().FullName));
                    throw ex;
                }
                finally
                {
                    watch.Stop();
                    var elapsedTime = (int)watch.ElapsedMilliseconds;
                    OpenTelemetryMetricsExtension.HTTP_REQUEST_ELAPSED_TIME.Record(elapsedTime, listTags.ToArray());
                }
            }
        }

        private string GetCompanyKey(HttpContext context)
        {
            StringValues companyKeyValue = default;
            context.Request.Headers.TryGetValue(ConstValues.KEY_COMPANY_KEY, out companyKeyValue);
            var companyKey = companyKeyValue.ToString();
            if (string.IsNullOrEmpty(companyKey))
                companyKey = "none";

            return companyKey;
        }

        private string GetCorrelationId(HttpContext context)
        {
            StringValues correlationIdValue = default;
            context.Request.Headers.TryGetValue(ConstValues.KEY_CORRELATION_ID, out correlationIdValue);
            var correlationId = correlationIdValue.ToString();
            if (string.IsNullOrEmpty(correlationId))
                correlationId = "none";

            return correlationId;
        }

        private string GetRoute(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint as RouteEndpoint;
            string routeValue = "none";
            if (endpoint != null)
                routeValue = endpoint.RoutePattern.RawText.ToLower();

            return routeValue;
        }
    }
}
