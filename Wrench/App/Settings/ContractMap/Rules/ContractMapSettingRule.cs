using App.Validations;

namespace App.Settings.ContractMap.Rules
{
    public class ContractMapSettingRule : IRule<ContractMapSetting>
    {
        public ValidateResult Do(ContractMapSetting value)
        {
            var result = ValidateResult.Create();

            if (string.IsNullOrEmpty(value.Id))
                result.AddError("ContractMapSetting.Id is required");

            if (value.MapFromTo?.Count() > 0)
            {
                foreach (var item in value.MapFromTo)
                {
                    var mapSplited = item.Split(':');
                    if(mapSplited.Length != 2)
                        result.AddError("ContractMapSetting.MapFromTo should separated with ':' ex 'customer.name:customer.fullName'");
                }
            }
            else
                result.AddError("ContractMapSetting.MapFromTo is required");



            return result;
        }
    }
}
