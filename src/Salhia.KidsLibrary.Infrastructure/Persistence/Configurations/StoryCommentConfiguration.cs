using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryCommentConfiguration : IEntityTypeConfiguration<StoryComment>
{
    public void Configure(EntityTypeBuilder<StoryComment> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);
        
        // ULID Configuration
        builder.Property(c => c.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(c => c.Content)
            .HasMaxLength(2000);

        // Foreign Key Configuration
        builder.Property(c => c.MasterStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(c => c.CreatedBy)
            .HasMaxLength(26);

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many Comments belong to One MasterStory
        builder.HasOne(c => c.MasterStory)
            .WithMany(ms => ms.Comments)
            .HasForeignKey(c => c.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // User navigation properties
        builder.HasOne(c => c.CreatedByUser)
            .WithMany()
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.UpdatedByUser)
            .WithMany()
            .HasForeignKey(c => c.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(c => c.Id); // For time-based ordering (ULID)
        builder.HasIndex(c => c.MasterStoryId); // For efficient lookups by story
        builder.HasIndex(c => c.CreatedAt);
    }
}
