using CleanAspCore.Data.Models.Departments;
using CleanAspCore.Data.Models.Employees;
using CleanAspCore.Data.Models.Jobs;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Data;

public class HrContext : DbContext
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Job> Jobs => Set<Job>();

    private readonly string? _connectionString;

    public HrContext() { }

    public HrContext(DbContextOptions<HrContext> context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

public static class HrContextExtensions
{
    public static void EnsureHrContextDatabaseIsCreated(this IHost host)
    {
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.EnsureCreated();
    }

    public static void MigrateHrContextDatabase(this IHost host)
    {
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.Migrate();
    }
}
