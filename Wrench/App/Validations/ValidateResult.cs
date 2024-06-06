using System.Text;

namespace App.Validations
{
    public class ValidateResult
    {
        public ValidateResult()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; private set; }

        public void AddError(string error)
            => Errors.Add(error);

        public void AddErrors(IEnumerable<string> errors)
            => Errors.AddRange(errors);

        public bool HasError => Errors.Count > 0;
        public bool IsSuccess => !HasError;
        public void Concate(ValidateResult validation)
            => Errors.AddRange(validation.Errors);

        public override string ToString()
        {
            if (IsSuccess)
                return "Success";

            var msgErrors = new StringBuilder();

            foreach (var error in Errors)
                msgErrors.AppendLine(error.ToString());

            return msgErrors.ToString();
        }


        public static ValidateResult Create() => new ValidateResult();
        public static ValidateResult Concate(params ValidateResult[] validations)
        {
            var result = Create();
            foreach (var validation in validations)
                result.AddErrors(validation.Errors);

            return result;
        }
    }
}
