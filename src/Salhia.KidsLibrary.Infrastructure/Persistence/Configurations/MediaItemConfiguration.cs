using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
{
    public void Configure(EntityTypeBuilder<MediaItem> builder)
    {
        // Primary Key
        builder.HasKey(mi => mi.Id);
        
        // ULID Configuration
        builder.Property(mi => mi.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(mi => mi.Title)
            .HasMaxLength(200);

        builder.Property(mi => mi.Description)
            .HasMaxLength(1000);

        builder.Property(mi => mi.Url)
            .HasMaxLength(500);

        // Foreign Key Configuration
        builder.Property(mi => mi.MasterStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(mi => mi.CreatedBy)
            .HasMaxLength(26);

        builder.Property(mi => mi.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many MediaItems belong to One MasterStory
        builder.HasOne(mi => mi.MasterStory)
            .WithMany()
            .HasForeignKey(mi => mi.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // User navigation properties
        builder.HasOne(mi => mi.CreatedByUser)
            .WithMany()
            .HasForeignKey(mi => mi.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mi => mi.UpdatedByUser)
            .WithMany()
            .HasForeignKey(mi => mi.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(mi => mi.Id); // For time-based ordering (ULID)
        builder.HasIndex(mi => mi.MasterStoryId); // For efficient lookups by story
        builder.HasIndex(mi => mi.Title);
        builder.HasIndex(mi => mi.CreatedAt);
    }
}
