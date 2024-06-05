using Wrench.HttpRequest.Settings.Actions.Types;
using Wrench.HttpRequest.Types;
using Wrench.Shared.Settings.Actions;
using Wrench.Shared.Validations;

namespace Wrench.HttpRequest.Settings.Actions
{
    public class ActionSetting : ActionBase, IValidable
    {
        public ActionType Type { get; set; }
        public HttpMethodType Method { get; set; }

        public string Url { get; set; }
        public bool Insecure { get; set; }

        public ValidateResult Valid()
        {
            return ValidateResult.Create();
        }
    }
}
