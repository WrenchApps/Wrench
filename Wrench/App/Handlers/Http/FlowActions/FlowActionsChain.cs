using App.Settings.Actions;
using App.Settings.ContractMap;
using App.Settings.ContractValidations;
using App.Settings.Strategies;

namespace App.Handlers.Http.FlowActions
{
    public static class FlowActionsChain
    {
        private static IDictionary<string, List<FlowAction>> FLOW_ACTION
            = new Dictionary<string, List<FlowAction>>();

        public static void StartFlowActions(string flowActionId)
            => FLOW_ACTION[flowActionId] = new List<FlowAction>();

        public static void PutFlowAction(
            string flowActionId,
            Type handleType,
            ActionSetting actionSetting = null,
            ContractMapSetting contractMap = null,
            ContractValidation contractValidation = null,
            CacheSetting cacheSetting = null,
            HttpIdempotencySetting httpIdempotencySetting = null)

           => FLOW_ACTION[flowActionId].Add(
               new FlowAction
               {
                   Order = FLOW_ACTION[flowActionId].Count + 1,
                   HandleType = handleType,
                   ActionSetting = actionSetting,
                   ContractMap = contractMap,
                   ContractValidation = contractValidation,
                   CacheSetting = cacheSetting,
                   HttpIdempotencySetting = httpIdempotencySetting
               });

        public static List<FlowAction> GetFlowActionsChain(string flowActionId)
            => FLOW_ACTION[flowActionId];
    }

    public class FlowAction
    {
        public int Order { get; set; }
        public Type HandleType { get; set; }
        public ActionSetting ActionSetting { get; set; }
        public ContractMapSetting ContractMap { get; set; }
        public ContractValidation ContractValidation { get; set; }
        public CacheSetting CacheSetting { get; set; }
        public HttpIdempotencySetting HttpIdempotencySetting { get; set; }
    }
}
