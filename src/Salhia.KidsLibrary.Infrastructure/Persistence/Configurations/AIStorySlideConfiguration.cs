using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class AIStorySlideConfiguration : IEntityTypeConfiguration<AIStorySlide>
{
    public void Configure(EntityTypeBuilder<AIStorySlide> builder)
    {
        // Primary Key
        builder.HasKey(ais => ais.Id);
        
        // ULID Configuration
        builder.Property(ais => ais.Id)
            .HasMaxLength(26);

        builder.Property(ais => ais.Title)
            .HasMaxLength(200);

        builder.Property(ais => ais.Description)
            .HasMaxLength(1000);

        builder.Property(ais => ais.ImagePrompt)
            .HasMaxLength(1000);

        builder.Property(ais => ais.ImageUrl)
            .HasMaxLength(500);

        builder.Property(ais => ais.Status)
            .HasDefaultValue(AIStorySlideStatus.Pending);

        // Foreign Key Configuration
        builder.Property(ais => ais.AIStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(ais => ais.CreatedBy)
            .HasMaxLength(26);

        builder.Property(ais => ais.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many AIStorySlides belong to One AIStory
        builder.HasOne(ais => ais.AIStory)
            .WithMany(ai => ai.AIStorySlides)
            .HasForeignKey(ais => ais.AIStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // User navigation properties
        builder.HasOne(ais => ais.CreatedByUser)
            .WithMany()
            .HasForeignKey(ais => ais.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ais => ais.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ais => ais.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ais => ais.AIStoryId); // For efficient lookups by AI story
        builder.HasIndex(ais => ais.Status);
    }
}
