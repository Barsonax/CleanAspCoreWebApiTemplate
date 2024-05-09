namespace CleanAspCore.Extensions.FluentValidation;

public class GenericNotNullRule : IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder) => builder.NotNull();
}
