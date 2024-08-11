namespace CleanAspCore.Common.NullableValidation;

internal interface IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder);
}
