using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Seeders;

public class SystemSettingSeeder : ICustomSeeder
{
    public int Order => SeederOrder.SystemSettings;

    public async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.SystemSettings.AnyAsync(cancellationToken))
        {
            context.SystemSettings.Add(new SystemSetting
            {
                AIStoryLimitCount = 1,
                AIStoryLimitDays = 7,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            });
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
