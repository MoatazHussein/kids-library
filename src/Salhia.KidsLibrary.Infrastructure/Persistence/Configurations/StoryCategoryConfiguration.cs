using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryCategoryConfiguration : IEntityTypeConfiguration<StoryCategory>
{
    public void Configure(EntityTypeBuilder<StoryCategory> builder)
    {
        // Primary Key
        builder.HasKey(sc => sc.Id);
        
        // ULID Configuration
        builder.Property(sc => sc.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(sc => sc.Title)
            .HasMaxLength(200);

        builder.Property(sc => sc.Description)
            .HasMaxLength(1000);

        builder.Property(sc => sc.ImageUrl)
            .HasMaxLength(500);

        // Audit Fields
        builder.Property(sc => sc.CreatedBy)
            .HasMaxLength(26);

        builder.Property(sc => sc.UpdatedBy)
            .HasMaxLength(26);

        // Indexes
        builder.HasIndex(sc => sc.Id); // For time-based ordering (ULID)
        builder.HasIndex(sc => sc.Title);
        builder.HasIndex(sc => sc.CreatedAt);
    }
}
