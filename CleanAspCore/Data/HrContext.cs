using CleanAspCore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Data;

public class HrContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Job> Jobs { get; set; }

    public HrContext()
    {
    }

    public HrContext(DbContextOptions<HrContext> context) : base(context)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=hr;Username=postgres;Password=postgres");
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
    public static void MigrateHrContext(this IHost host)
    {
        using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
        context.Database.Migrate();
    }
}
