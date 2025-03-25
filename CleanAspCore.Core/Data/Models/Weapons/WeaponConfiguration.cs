using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanAspCore.Core.Data.Models.Weapons;

internal sealed class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder
            .HasDiscriminator(c => c.Type)
            .HasValue<Bow>(Weapon.Types.Bow)
            .HasValue<Sword>(Weapon.Types.Sword);
    }
}
