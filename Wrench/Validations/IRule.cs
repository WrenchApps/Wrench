namespace Wrench.Validations
{
    public interface IRule<T> where T : IValidable
    {
        ValidateResult Do(T value);
    }
}
