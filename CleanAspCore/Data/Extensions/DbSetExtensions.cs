using CleanAspCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanAspCore.Data.Extensions;

public static class DbSetExtensions
{
    public static EntityEntry<Employee>? AddIfNotExists(this DbSet<Employee> dbSet, Employee entity) =>
        dbSet.Any(x => x.Id == entity.Id) ? null : dbSet.Add(entity);

    public static EntityEntry<Department>? AddIfNotExists(this DbSet<Department> dbSet, Department entity) =>
        dbSet.Any(x => x.Id == entity.Id) ? null : dbSet.Add(entity);
    public static EntityEntry<Job>? AddIfNotExists(this DbSet<Job> dbSet, Job entity) =>
        dbSet.Any(x => x.Id == entity.Id) ? null : dbSet.Add(entity);
}
