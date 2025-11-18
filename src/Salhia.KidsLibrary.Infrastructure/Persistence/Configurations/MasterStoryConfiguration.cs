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

        builder.Property(ms => ms.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(ms => ms.MediaUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ms => ms.MediaType)
            .HasDefaultValue(MediaType.Unknown);

        builder.Property(ms => ms.ApprovalStatus)
            .HasDefaultValue(ApprovalStatus.Pending);

        builder.Property(ms => ms.AuthorName)
            .HasMaxLength(200);

        // Foreign Key Configuration
        builder.Property(ms => ms.StoryCategoryId)
            .IsRequired()
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(ms => ms.CreatedBy)
            .HasMaxLength(26);

        builder.Property(ms => ms.UpdatedBy)
            .HasMaxLength(26);

        builder.HasOne(ms => ms.StoryCategory)
            .WithMany(sc => sc.MasterStories)
            .HasForeignKey(ms => ms.StoryCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ms => ms.CreatedByUser)
             .WithMany()
             .HasForeignKey(ms => ms.CreatedBy)
             .OnDelete(DeleteBehavior.NoAction); 

        builder.HasOne(ms => ms.UpdatedByUser)
            .WithMany()
            .HasForeignKey(ms => ms.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction); 


        // Indexes
        builder.HasIndex(ms => ms.Id); // For time-based ordering (ULID)
        builder.HasIndex(ms => ms.StoryCategoryId); // For efficient lookups by category
        builder.HasIndex(ms => ms.Title);
        builder.HasIndex(ms => ms.CreatedAt);
        builder.HasIndex(ms => ms.ApprovalStatus);
    }
}
