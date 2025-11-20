using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryViewSessionConfiguration : IEntityTypeConfiguration<StoryViewSession>
{
    public void Configure(EntityTypeBuilder<StoryViewSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(26);

        builder.Property(x => x.MasterStoryId)
            .IsRequired()
            .HasMaxLength(26);

        builder.Property(x => x.VisitorKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UserId)
            .HasMaxLength(26);

        builder.Property(x => x.LastViewAt)
            .IsRequired();

        builder.Property(x => x.ViewCount)
            .IsRequired()
            .HasDefaultValue(0);

        // Composite unique index - one session per visitor per story
        builder.HasIndex(x => new { x.MasterStoryId, x.VisitorKey })
            .IsUnique()
            .HasDatabaseName("IX_StoryViewSessions_MasterStoryId_VisitorKey");

        // Index on LastViewAt for cleanup queries
        builder.HasIndex(x => x.LastViewAt)
            .HasDatabaseName("IX_StoryViewSessions_LastViewAt");

        // Foreign Key: MasterStory - cascade delete when story is deleted
        builder.HasOne(x => x.MasterStory)
           .WithMany(x => x.ViewSessions)
           .HasForeignKey(x => x.MasterStoryId)
           .OnDelete(DeleteBehavior.Cascade);

        // Foreign Key: User (optional - for authenticated users)
        builder.HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        // Audit Fields
        builder.Property(x => x.CreatedBy)
            .IsRequired(false)
            .HasMaxLength(26);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(26);
    }
}
