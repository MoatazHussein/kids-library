using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // Configure ULID as primary key
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasMaxLength(26) // ULID string length
            .IsRequired();

        // Property configurations
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).HasMaxLength(50);
        builder.Property(u => u.Email).HasMaxLength(256);

        builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(15);

        // Indexes
        builder.HasIndex(u => u.PhoneNumber)
             .IsUnique()
             .HasFilter("[PhoneNumber] IS NOT NULL");

        // Index on Id for efficient time-based ordering (ULID is chronologically sortable)
        builder.HasIndex(u => u.Id);

        // Index on CreatedAt for time-based queries
        builder.HasIndex(u => u.CreatedAt);
    }
}
