using CleanAspCore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanAspCore.Data;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(c => c.Id)
            .HasConversion(c => c.Value, c => new(c));

        builder.Property(c => c.Email)
            .HasConversion(c => c.Email, c => new (c));
    }
}
