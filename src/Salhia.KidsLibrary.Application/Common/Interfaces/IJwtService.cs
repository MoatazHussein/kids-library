using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Interfaces;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(AppUser user, IEnumerable<string> roles);

}
