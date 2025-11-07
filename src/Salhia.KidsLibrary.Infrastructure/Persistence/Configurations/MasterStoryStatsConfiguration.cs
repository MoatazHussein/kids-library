using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class MasterStoryStatsConfiguration : IEntityTypeConfiguration<MasterStoryStats>
{
    public void Configure(EntityTypeBuilder<MasterStoryStats> builder)
    {
        builder.Property(mss => mss.Id)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(mss => mss.MasterStoryId)
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(mss => mss.RatingsCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(mss => mss.RatingsSum)
            .IsRequired()
            .HasDefaultValue(0);

        // One-to-one relationship with MasterStory
        builder.HasOne(mss => mss.MasterStory)
            .WithOne()
            .HasForeignKey<MasterStoryStats>(mss => mss.MasterStoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique index on MasterStoryId - one stats record per story
        builder.HasIndex(mss => mss.MasterStoryId)
            .IsUnique();
    }
}
