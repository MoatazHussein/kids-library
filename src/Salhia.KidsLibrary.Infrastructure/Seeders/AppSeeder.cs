using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Seeders;

internal class AppSeeder(AppDbContext dbContext, IEnumerable<ICustomSeeder> seeders) : IAppSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            var orderedSeeders = seeders.OrderBy(s => s.Order);

            foreach (var seeder in orderedSeeders)
            {
                await seeder.SeedAsync(dbContext, CancellationToken.None);
            }
        }
    }
}
