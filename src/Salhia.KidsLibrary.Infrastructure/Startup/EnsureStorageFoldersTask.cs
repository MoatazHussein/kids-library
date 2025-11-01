using Salhia.KidsLibrary.Application.Common.Interfaces;

namespace Salhia.KidsLibrary.Infrastructure.Startup;

public class EnsureStorageFoldersTask(IStorageService storageService) : IStartupTask
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await storageService.EnsureImageDirectoryExistsAsync(cancellationToken);
    }
}
