using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class CustomStoryConfiguration : IEntityTypeConfiguration<CustomStory>
{
    public void Configure(EntityTypeBuilder<CustomStory> builder)
    {
        // Primary Key
        builder.HasKey(cs => cs.Id);
        
        // ULID Configuration
        builder.Property(cs => cs.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(cs => cs.Title)
            .HasMaxLength(200);

        builder.Property(cs => cs.AuthorName)
            .HasMaxLength(200);

        builder.Property(cs => cs.Description)
            .HasMaxLength(1000);

        builder.Property(cs => cs.ImageUrl)
            .HasMaxLength(500);

        // Audit Fields
        builder.Property(cs => cs.CreatedBy)
            .HasMaxLength(26);

        builder.Property(cs => cs.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: One CustomStory has Many CustomStoryItems
        builder.HasMany(cs => cs.CustomStoryItems)
            .WithOne(csi => csi.CustomStory)
            .HasForeignKey(csi => csi.CustomStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // User navigation properties
        builder.HasOne(cs => cs.CreatedByUser)
            .WithMany()
            .HasForeignKey(cs => cs.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cs => cs.UpdatedByUser)
            .WithMany()
            .HasForeignKey(cs => cs.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(cs => cs.Id); // For time-based ordering (ULID)
        builder.HasIndex(cs => cs.Title);
        builder.HasIndex(cs => cs.CreatedAt);
    }
}
