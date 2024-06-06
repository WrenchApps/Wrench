using App.Settings.Actions;
using App.Settings.Connections;
using App.Settings.ContractMap;
using App.Settings.ContractValidations;
using App.Settings.Entrypoints;
using App.Settings.Strategies;
using App.Validations;

namespace App.Settings
{
    public class ApplicationSetting
    {
        public static ApplicationSetting Current { get; private set; }

        public ApplicationSetting()
        {
            if (Current == null)
                Current = this;
        }

        public StartupSetting Startup { get; set; }
        public ConnectionSetting Connections { get; set; }
        public EntrypointSetting Entrypoints { get; set; }
        public List<ActionSetting> Actions { get; set; }
        public List<FlowActionsSetting> FlowActions { get; set; }
        public List<ContractMapSetting> ContractMaps { get; set; }
        public List<ContractValidation> ContractValidations { get; set; }
        public StrategiesSetting Strategies { get; set; }


        public List<IValidable> GetValidables()
        {
            var validables = new List<IValidable>();

            if(Startup != null)
                validables.Add(Startup);

            if (Entrypoints != null)
                validables.Add(Entrypoints);

            if (Entrypoints.Routes != null)
                validables.AddRange(Entrypoints.Routes);

            if (Actions != null)
                validables.AddRange(Actions);

            if (FlowActions != null)
                validables.AddRange(FlowActions);

            if (ContractMaps != null)
                validables.AddRange(ContractMaps);

            if (ContractValidations != null)
                validables.AddRange(ContractValidations);

            if (Connections != null)
                validables.Add(Connections);

            if (Strategies != null)
                validables.Add(Strategies);

            return validables;
        }
    }
}
