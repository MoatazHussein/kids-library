using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class StoryShareConfiguration : IEntityTypeConfiguration<StoryShare>
{
    public void Configure(EntityTypeBuilder<StoryShare> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .HasMaxLength(26);

        builder.Property(x => x.MasterStoryId)
            .IsRequired()
            .HasMaxLength(26);
            
         builder.Property(x => x.Platform)
            .IsRequired()
            .HasDefaultValue(SharePlatform.Unknown);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45); // IPv6 max length

        // Index for querying shares by story
        builder.HasIndex(x => x.MasterStoryId)
            .HasDatabaseName("IX_StoryShares_MasterStoryId");

        // Index for querying shares by user (for authenticated users)
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_StoryShares_UserId")
            .HasFilter("[UserId] IS NOT NULL"); // Filtered index for non-null UserId

        // Foreign Key: User (nullable for anonymous shares)
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull); // Keep share record if user deleted

        // Foreign Key: MasterStory
        builder.HasOne(x => x.MasterStory)
            .WithMany(x => x.Shares)
            .HasForeignKey(x => x.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
