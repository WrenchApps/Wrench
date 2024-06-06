using App.Settings.Actions.Types;
using App.Settings.Actions;
using App.Settings.Entrypoints.Routes;
using App.Settings;
using App.Settings.Strategies;

namespace App.Handlers.Http.FlowActions
{
    public static class EntrypointHttpFlowActionsChainBuilder
    {
        public static void Builder(RouteSetting routeSetting)
        {
            var appSetting = ApplicationSetting.Current;

            var flowActionId = routeSetting.GetFlowActionId;

            FlowActionsChain.StartFlowActions(flowActionId);

            var flowAction = appSetting.FlowActions?.FirstOrDefault(f => f.Id == flowActionId);

            if (flowAction != null)
            {
                var actionsSetting = new List<ActionSetting>();

                foreach (var actionId in flowAction.ActionsId)
                {
                    var action = appSetting.Actions.FirstOrDefault(a => a.Id == actionId);
                    actionsSetting.Add(action);
                }

                foreach (var action in actionsSetting)
                {
                    if (string.IsNullOrEmpty(action.RequestContractValidationId) == false)
                    {
                        var contractValidation = appSetting.ContractValidations.FirstOrDefault(a => a.Id == action.RequestContractValidationId);
                        FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpRequestContractValidationHandler), contractValidation: contractValidation);
                    }

                    if (string.IsNullOrEmpty(action.StrategyHttpIdempotencyId) == false)
                    {
                        var httpIdempotencySetting = appSetting.Strategies.HttpIdempotencies.FirstOrDefault(i => i.Id == action.StrategyHttpIdempotencyId);
                        FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpRequestStrategyHttpIdempotencyHandler), httpIdempotencySetting: httpIdempotencySetting);
                        FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpRequestStrategyCacheHandler), cacheSetting: new CacheSetting { Ttl = httpIdempotencySetting.Ttl });
                    }

                    if (string.IsNullOrEmpty(action.StrategyCacheId) == false)
                    {
                        var cacheSetting = appSetting.Strategies.Caches.FirstOrDefault(a => a.Id == action.StrategyCacheId);
                        FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpRequestStrategyCacheHandler), cacheSetting: cacheSetting);
                    }

                    if (action.Type == ActionType.HttpRequest)
                    {
                        FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpClientRequestHandler), actionSetting: action);

                        if (action.ReponseContractMapId != null)
                        {
                            var contractMap = appSetting.ContractMaps.FirstOrDefault(a => a.Id == action.ReponseContractMapId);
                            FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpResponseContractMapHandler), contractMap: contractMap);
                        }
                    }
                }

                if (routeSetting.ResponseMock == null)
                    FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpWriteResponseHandler));
            }

            if (routeSetting.ResponseMock != null)
                FlowActionsChain.PutFlowAction(flowActionId, typeof(HttpWriteResponseMockHandler));
        }
    }
}
