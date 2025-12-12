using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Seeders;

public class RoleSeeder : ICustomSeeder
{
    public int Order => SeederOrder.Roles;

    public async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Roles.AnyAsync(cancellationToken))
        {
            var roles = GetRoles();
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private IEnumerable<AppRole> GetRoles()
    {
        return
        [
            new AppRole
            {
                Id = Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Admin.ToLower()),
                NormalizedName = UserRoles.Admin.ToUpper(),
            },
            new AppRole
            {
                Id = Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Teacher.ToLower()),
                NormalizedName = UserRoles.Teacher.ToUpper(),
            },
            new AppRole
            {
                Id = Ulid.NewUlid().ToString(),
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UserRoles.Student.ToLower()),
                NormalizedName = UserRoles.Student.ToUpper(),
            },
        ];
    }
}
