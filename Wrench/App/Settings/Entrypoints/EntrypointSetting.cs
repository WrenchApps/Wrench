using App.Settings.Entrypoints.Routes;
using App.Validations;

namespace App.Settings.Entrypoints
{
    public class EntrypointSetting : IValidable
    {
        public List<RouteSetting> Routes { get; set; }

        public ValidateResult Valid()
        {
            var result = ValidateResult.Create();

            return result;
        }
    }
}
