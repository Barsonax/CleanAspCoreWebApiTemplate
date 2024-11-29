using FluentValidation;

namespace CleanAspCore.Core.Common.NullableValidation;

internal sealed class GenericNotNullRule : IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder) => builder.NotNull();
}
