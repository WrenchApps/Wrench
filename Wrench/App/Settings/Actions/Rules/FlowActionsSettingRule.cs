using App.Validations;
using System;

namespace App.Settings.Actions.Rules
{
    public class FlowActionsSettingRule : IRule<FlowActionsSetting>
    {
        public ValidateResult Do(FlowActionsSetting value)
        {
            var result = ValidateResult.Create();
            if (string.IsNullOrEmpty(value.Id))
                result.AddError("FlowActionsSetting.Id is required");


            if (value.ActionsId?.Count() > 0)
            {
                var appSetting = ApplicationSetting.Current;
                foreach (var actionId in value.ActionsId)
                {
                    var hasActionId = appSetting.Actions.FirstOrDefault(a => a.Id == actionId) != null;

                    if (hasActionId is false)
                      result.AddError($"FlowActionsSetting.ActionsId {actionId} should be configured");
                }
            }
            else
                result.AddError($"FlowActionsSetting.ActionsId is required");

            return result;
        }
    }
}
