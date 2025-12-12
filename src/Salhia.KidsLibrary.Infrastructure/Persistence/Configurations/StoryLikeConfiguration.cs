using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryLikeConfiguration : IEntityTypeConfiguration<StoryLike>
{
    public void Configure(EntityTypeBuilder<StoryLike> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(26);

        builder.Property(x => x.MasterStoryId)
            .IsRequired()
            .HasMaxLength(26);

        // Composite unique index - one like per user per story
        builder.HasIndex(x => new { x.UserId, x.MasterStoryId })
            .IsUnique()
            .HasDatabaseName("IX_StoryLikes_UserId_MasterStoryId");

        // Foreign Key: User
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Foreign Key: MasterStory
        builder.HasOne(x => x.MasterStory)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
