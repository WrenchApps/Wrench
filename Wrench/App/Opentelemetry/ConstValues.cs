namespace Opentelemetry
{
    internal static class ConstValues
    {
        public const string KEY_CORRELATION_ID = "x-correlation-id";
        public const string KEY_COMPANY_KEY = "x-company-key";
        public const string KEY_HTTP_ROUTE = "http_route";
        public const string KEY_HTTP_METHOD = "http_method";
        public const string KEY_STATUS_CODE = "http_status_code";
        public const string KEY_STATUS_CODE_FAMILY = "http_status_code_family";
        public const string EXCEPTION_FULL_NAME = "exception_full_name";

        public const int MIN_METRICS_INTERVAL_SECONDS = 10;
    }
}