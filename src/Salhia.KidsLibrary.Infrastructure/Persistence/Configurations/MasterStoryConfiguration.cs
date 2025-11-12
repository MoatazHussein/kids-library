using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class MasterStoryConfiguration : IEntityTypeConfiguration<MasterStory>
{
    public void Configure(EntityTypeBuilder<MasterStory> builder)
    {
        // Primary Key
        builder.HasKey(ms => ms.Id);
        
        // ULID Configuration
        builder.Property(ms => ms.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(ms => ms.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ms => ms.Content)
            .HasMaxLength(5000);

        builder.Property(ms => ms.ImageUrl)
            .HasMaxLength(500);

        builder.Property(ms => ms.ApprovalStatus)
            .HasDefaultValue(ApprovalStatus.Pending);

        // Foreign Key Configuration
        builder.Property(ms => ms.StoryCategoryId)
            .IsRequired()
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(ms => ms.CreatedBy)
            .HasMaxLength(26);

        builder.Property(ms => ms.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many MasterStories belong to One StoryCategory
        builder.HasOne(ms => ms.StoryCategory)
            .WithMany(sc => sc.MasterStories)
            .HasForeignKey(ms => ms.StoryCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Author navigation (CreatedBy is the Author)
        builder.HasOne(ms => ms.Author)
            .WithMany()
            .HasForeignKey(ms => ms.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // UpdatedByUser navigation
        builder.HasOne(ms => ms.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ms => ms.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // MediaItems navigation
        builder.HasMany(ms => ms.MediaItems)
            .WithOne(mi => mi.MasterStory)
            .HasForeignKey(mi => mi.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(ms => ms.Id); // For time-based ordering (ULID)
        builder.HasIndex(ms => ms.StoryCategoryId); // For efficient lookups by category
        builder.HasIndex(ms => ms.Title);
        builder.HasIndex(ms => ms.CreatedAt);
        builder.HasIndex(ms => ms.ApprovalStatus);
    }
}
