using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanAspCore;

public static class DbSetExtensions
{
    public static EntityEntry<T>? AddIfNotExists<T>(this DbSet<T> dbSet, T entity)
        where T : Entity
    {
        return !dbSet.Any(x => x.Id == entity.Id) ? dbSet.Add(entity) : null;
    }
}