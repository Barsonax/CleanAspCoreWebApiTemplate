namespace CleanAspCore.Extensions.FluentValidation;

public class GenericNotNullOrEmptyRule : IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder) => builder.NotNull().NotEmpty();
}
