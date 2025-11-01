using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Seeders;

internal class AppSeeder(AppDbContext dbContext) : IAppSeeder
{
    public async Task Seed()
    {

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }

        }
    }

    private IEnumerable<AppRole> GetRoles()
    {
        List<AppRole> roles =
            [

            new AppRole
            {
                Id =  Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Admin.ToLower()),
                NormalizedName  = UserRoles.Admin.ToUpper(),
            },
            new AppRole
            {
                Id =  Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Teacher.ToLower()),
                NormalizedName = UserRoles.Teacher.ToUpper(),
            },
            new AppRole
            {
                Id =  Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Student.ToLower()),
                NormalizedName = UserRoles.Student.ToUpper(),
            },
            ];

        return roles;
    }
}
