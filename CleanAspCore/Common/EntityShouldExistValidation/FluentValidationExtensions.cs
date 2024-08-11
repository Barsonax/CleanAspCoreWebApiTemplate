using CleanAspCore.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Common.EntityShouldExistValidation;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<TModel, Guid> EntityShouldExist<TModel, TEntity>(this IRuleBuilder<TModel, Guid> rule, DbSet<TEntity> entities)
        where TEntity : class, IEntity =>
        rule.MustAsync((id, t) => entities.AnyAsync(y => y.Id == id, t))
            .WithMessage((x, y) => $"{typeof(TEntity).Name} with id {y} does not exist.");
}
