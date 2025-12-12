using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Seeders;

public interface ICustomSeeder
{
    int Order { get; }
    Task SeedAsync(AppDbContext context, CancellationToken cancellationToken);
}
