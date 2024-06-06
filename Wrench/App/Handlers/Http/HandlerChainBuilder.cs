using App.Contexts;
using App.Handlers.Http.FlowActions;

namespace App.Handlers.Http
{
    public static class HandlerChainBuilder
    {
        public static Handler ChainBuilder(StepladderHttpContext context)
        {
            var routeSetting = context.RouteSetting;

            var flowActions = FlowActionsChain.GetFlowActionsChain(routeSetting.GetFlowActionId);

            Handler firstHandler = new HttpFirstHandler();
            var current = firstHandler;

            foreach (var flowAction in flowActions)
            {
                var handler = context.HttpContext.RequestServices.GetService(flowAction.HandleType) as Handler;

                if (flowAction.ActionSetting != null)
                    handler.ActionSetting = flowAction.ActionSetting;

                if (flowAction.ContractValidation != null)
                    handler.ContractValidation = flowAction.ContractValidation;

                if (flowAction.ContractMap != null)
                    handler.ContractMap = flowAction.ContractMap;

                if (flowAction.CacheSetting != null)
                    handler.CacheSetting = flowAction.CacheSetting;

                if(flowAction.HttpIdempotencySetting != null)
                    handler.HttpIdempotencySetting = flowAction.HttpIdempotencySetting;

                current.SetNext(handler);
                current = handler;
            }

            return firstHandler;
        }
    }
}
