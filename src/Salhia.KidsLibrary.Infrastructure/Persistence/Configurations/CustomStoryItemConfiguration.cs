using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class CustomStoryItemConfiguration : IEntityTypeConfiguration<CustomStoryItem>
{
    public void Configure(EntityTypeBuilder<CustomStoryItem> builder)
    {
        // Primary Key
        builder.HasKey(csi => csi.Id);
        
        // ULID Configuration
        builder.Property(csi => csi.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(csi => csi.Title)
            .HasMaxLength(200);

        builder.Property(csi => csi.Description)
            .HasMaxLength(1000);

        builder.Property(csi => csi.ImageUrl)
            .HasMaxLength(500);

        // Foreign Key Configuration
        builder.Property(csi => csi.CustomStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(csi => csi.CreatedBy)
            .HasMaxLength(26);

        builder.Property(csi => csi.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many CustomStoryItems belong to One CustomStory
        builder.HasOne(csi => csi.CustomStory)
            .WithMany(cs => cs.CustomStoryItems)
            .HasForeignKey(csi => csi.CustomStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // User navigation properties
        builder.HasOne(csi => csi.CreatedByUser)
            .WithMany()
            .HasForeignKey(csi => csi.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(csi => csi.UpdatedByUser)
            .WithMany()
            .HasForeignKey(csi => csi.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(csi => csi.Id); // For time-based ordering (ULID)
        builder.HasIndex(csi => csi.CustomStoryId); // For efficient lookups by story
        builder.HasIndex(csi => csi.Title);
        builder.HasIndex(csi => csi.CreatedAt);
    }
}
