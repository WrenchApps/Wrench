using System.Data;
using Wrench.Settings.Actions.Types;
using Wrench.Types;
using Wrench.Validations;

namespace Wrench.Settings.Actions
{
    public class ActionSetting : IValidable
    {
        public string Id { get; set; }
        public ActionType Type { get; set; }
        public HttpMethodType Method { get; set; }

        #region httpRequest
        public string CertificateId { get; set; }
        public string Url { get; set; }
        public List<ActionRouteMap> RouteMaps { get; set; } = new List<ActionRouteMap>();
        public List<ActionHeaderMap> HeaderMaps { get; set; } = new List<ActionHeaderMap>();
        public List<ActionHeaderMap> ResponseHeaderMaps { get; set; } = new List<ActionHeaderMap>();
        public string HttpClientAuthenticationId { get; set; }
        public List<HttpResponseContractMapSetting> HttpResponseContractMap { get; set; }
        public bool IsProxy { get; set; }
        public bool Insecure { get; set; }
        #endregion

        
        public string BeforeActionContractMapId { get; set; }
        public string AfterActionContractMapId { get; set; }
        public bool AsyncMode { get; set; }
        public List<StatusCodeMap> StatusCodeMaps { get; set; }

        public ValidateResult Valid()
        {
            var rules = new IRule<ActionSetting>[]
            {
                new ActionSettingRule(),
                new ActionSettingTypeHttpRequestRule(),
                new ActionSettingTypeProducerKafkaMessageRule(),
                new ActionSettingTypeRedisRule(),
                new ActionSettingTypeS3Rule(),
                new ActionSettingTypeDynamoDbRule()
            };

            var results = RuleExecute.Execute(this, rules);
            foreach (var route in RouteMaps)
                results.Concate(route.Valid());

            foreach (var header in HeaderMaps)
                results.Concate(header.Valid());

            foreach (var header in ResponseHeaderMaps)
                results.Concate(header.Valid());

            if (HttpResponseContractMap != null)
                foreach (var httpResponseContract in HttpResponseContractMap)
                    results.Concate(httpResponseContract.Valid());

            if (StatusCodeMaps != null)
                foreach (var statusCodeMaps in StatusCodeMaps)
                    results.Concate(statusCodeMaps.Valid());

            return results;
        }
    }
}
