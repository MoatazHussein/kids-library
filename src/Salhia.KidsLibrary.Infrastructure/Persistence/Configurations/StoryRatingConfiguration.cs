using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryRatingConfiguration : IEntityTypeConfiguration<StoryRating>
{
    public void Configure(EntityTypeBuilder<StoryRating> builder)
    {
        builder.Property(sr => sr.Id)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(sr => sr.UserId)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(sr => sr.MasterStoryId)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(sr => sr.Rating)
            .IsRequired();

        // Relationships
        builder.HasOne(sr => sr.User)
            .WithMany()
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sr => sr.MasterStory)
            .WithMany(ms => ms.Ratings)
            .HasForeignKey(sr => sr.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite unique index: one rating per user per story
        builder.HasIndex(sr => new { sr.UserId, sr.MasterStoryId })
            .IsUnique();

        // Indexes for performance
        builder.HasIndex(sr => sr.MasterStoryId);
        builder.HasIndex(sr => sr.UserId);
    }
}
