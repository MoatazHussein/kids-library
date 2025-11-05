using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class FavoriteStoryConfiguration : IEntityTypeConfiguration<FavoriteStory>
{
    public void Configure(EntityTypeBuilder<FavoriteStory> builder)
    {
        // Primary Key
        builder.HasKey(fs => fs.Id);
        
        // ULID Configuration
        builder.Property(fs => fs.Id)
            .HasMaxLength(26);

        // Foreign Key Configuration
        builder.Property(fs => fs.UserId)
            .HasMaxLength(26);

        builder.Property(fs => fs.MasterStoryId)
            .HasMaxLength(26);

        // Audit Fields
        builder.Property(fs => fs.CreatedBy)
            .HasMaxLength(26);

        builder.Property(fs => fs.UpdatedBy)
            .HasMaxLength(26);

        // Relationship: Many FavoriteStories belong to One User
        builder.HasOne(fs => fs.User)
            .WithMany()
            .HasForeignKey(fs => fs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: Many FavoriteStories belong to One MasterStory
        builder.HasOne(fs => fs.MasterStory)
            .WithMany()
            .HasForeignKey(fs => fs.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite unique index to prevent duplicate favorites
        builder.HasIndex(fs => new { fs.UserId, fs.MasterStoryId })
            .IsUnique();

        // Indexes
        builder.HasIndex(fs => fs.Id); // For time-based ordering (ULID)
        builder.HasIndex(fs => fs.UserId); // For efficient lookups by user
        builder.HasIndex(fs => fs.MasterStoryId); // For efficient lookups by story
        builder.HasIndex(fs => fs.CreatedAt); // For sorting by favorited date
    }
}
