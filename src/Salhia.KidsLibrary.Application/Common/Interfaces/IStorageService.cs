using Microsoft.AspNetCore.Http;

namespace Salhia.KidsLibrary.Application.Common.Interfaces;

public interface IStorageService
{
    Task EnsureImageDirectoryExistsAsync(CancellationToken cancellationToken = default);
    Task<string> SaveImageAsync(IFormFile file, CancellationToken cancellationToken);

}
