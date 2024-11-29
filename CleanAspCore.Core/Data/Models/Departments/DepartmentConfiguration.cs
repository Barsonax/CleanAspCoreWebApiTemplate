using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanAspCore.Core.Data.Models.Departments;

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(100);

        builder.Property(c => c.City)
            .HasMaxLength(100);
    }
}
