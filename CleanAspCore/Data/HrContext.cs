using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.EntityFrameworkCore;

namespace CleanAspCore.Data;

public class HrContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Job> Jobs { get; set; }
    
    public HrContext() { }
    public HrContext(DbContextOptions<HrContext> context) : base(context) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=hr;Username=postgres;Password=postgres");    
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<Employee>()
            .Property(x => x.Id)
            .ValueGeneratedNever();
        
        modelBuilder.Entity<Department>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<Department>()
            .Property(x => x.Id)
            .ValueGeneratedNever();
        
        modelBuilder.Entity<Job>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<Job>()
            .Property(x => x.Id)
            .ValueGeneratedNever();
    }
}