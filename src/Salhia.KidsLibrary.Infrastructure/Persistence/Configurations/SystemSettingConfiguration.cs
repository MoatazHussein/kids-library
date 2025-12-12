using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Infrastructure.Persistence.Configurations;

public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AIStoryLimitCount)
            .HasDefaultValue(1);

        builder.Property(x => x.AIStoryLimitDays)
            .HasDefaultValue(7);
    }
}
