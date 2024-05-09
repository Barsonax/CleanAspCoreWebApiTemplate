using System.Linq.Expressions;
using System.Reflection;

namespace CleanAspCore.Extensions.FluentValidation;

public static class FluentValidationExtensions
{
    public static void ValidateNullableReferences<TModel>(this AbstractValidator<TModel> validator)
    {
        IEnumerable<PropertyInfo> properties = GetNonNullableProperties<TModel>(new NullabilityInfoContext());

        validator.ApplyRuleToProperties(new GenericNotNullRule(), properties);
    }

    private static IEnumerable<PropertyInfo> GetNonNullableProperties<TModel>(NullabilityInfoContext nullabilityInfoContext) =>
        typeof(TModel)
            .GetProperties()
            .Where(x => nullabilityInfoContext.Create(x).WriteState == NullabilityState.NotNull);

    private static void ApplyRuleInternal<TModel, TProperty>(AbstractValidator<TModel> validator, IGenericRule rule, PropertyInfo propertyInfo)
    {
        var builder = CreateRuleBuilder<TModel, TProperty>(validator, propertyInfo);
        rule.ApplyRule(builder);
    }

    private static IRuleBuilderInitial<TModel, TProperty> CreateRuleBuilder<TModel, TProperty>(AbstractValidator<TModel> validator, PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(propertyInfo);

        ParameterExpression entityParam = Expression.Parameter(typeof(TModel), "x");
        Expression columnExpr = Expression.Property(entityParam, propertyInfo);

        return validator.RuleFor(Expression.Lambda<Func<TModel, TProperty>>(columnExpr, entityParam));
    }

    public static void ApplyRuleToProperties<TModel>(this AbstractValidator<TModel> validator, IGenericRule rule, IEnumerable<PropertyInfo> properties)
    {
        foreach (PropertyInfo property in properties)
        {
            validator.ApplyRuleToProperty(rule, property);
        }
    }

    public static void ApplyRuleToProperty<TModel>(this AbstractValidator<TModel> validator, IGenericRule rule, PropertyInfo property)
    {
        MethodInfo methodInfo = typeof(FluentValidationExtensions).GetMethod(nameof(ApplyRuleInternal), BindingFlags.Static | BindingFlags.NonPublic)!;
        Type[] argumentTypes = [typeof(TModel), property.PropertyType];
        MethodInfo genericMethod = methodInfo.MakeGenericMethod(argumentTypes);
        genericMethod.Invoke(null, [validator, rule, property]);
    }
}
