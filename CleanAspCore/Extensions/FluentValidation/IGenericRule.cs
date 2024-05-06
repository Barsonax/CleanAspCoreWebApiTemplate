namespace CleanAspCore.Extensions.FluentValidation;

public interface IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder);
}
