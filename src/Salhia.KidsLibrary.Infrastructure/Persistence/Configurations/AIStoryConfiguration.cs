using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class AIStoryConfiguration : IEntityTypeConfiguration<AIStory>
{
    public void Configure(EntityTypeBuilder<AIStory> builder)
    {
        // Primary Key
        builder.HasKey(ai => ai.Id);
        
        // ULID Configuration
        builder.Property(ai => ai.Id)
            .HasMaxLength(26);

        // Property Configurations
        builder.Property(ai => ai.StoryName)
            .HasMaxLength(200);

        builder.Property(ai => ai.HeroName)
            .HasMaxLength(200);

        builder.Property(ai => ai.HeroImageUrl)
            .HasMaxLength(500);

        // Foreign Key Configuration
        builder.Property(ai => ai.CustomStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(ai => ai.CreatedBy)
            .HasMaxLength(26);

        builder.Property(ai => ai.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: One AIStory has Many AIStorySlides
        builder.HasMany(ai => ai.AIStorySlides)
            .WithOne(ais => ais.AIStory)
            .HasForeignKey(ais => ais.AIStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: One CustomStory has Many AIStories
        builder.HasOne(ai => ai.CustomStory)
            .WithMany()
            .HasForeignKey(ai => ai.CustomStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // User navigation properties
        builder.HasOne(ai => ai.CreatedByUser)
            .WithMany()
            .HasForeignKey(ai => ai.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ai => ai.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ai => ai.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ai => ai.CustomStoryId); // For efficient lookups by custom story
    }
}
