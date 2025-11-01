using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        // Configure ULID as primary key
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasMaxLength(26) // ULID string length
            .IsRequired();

        // Property configurations
        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.NormalizedName)
            .HasMaxLength(50);

        // Index on Id for efficient time-based ordering (ULID is chronologically sortable)
        builder.HasIndex(r => r.Id);

        // Index on NormalizedName for efficient role lookups
        builder.HasIndex(r => r.NormalizedName)
            .IsUnique()
            .HasFilter("[NormalizedName] IS NOT NULL");
    }
}
