namespace Wrench.Shared.Validations
{
    public static class RuleExecute
    {
        public static ValidateResult Execute<T>(T validable, params IRule<T>[] validates)
            where T : IValidable
        {
            var finalResult = ValidateResult.Create();
            foreach (var validation in validates)
            {
                var result = validation.Do(validable);
                finalResult.Concate(result);
            }

            return finalResult;
        }
    }
}
