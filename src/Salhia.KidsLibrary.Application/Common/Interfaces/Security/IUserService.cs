using System.Linq.Expressions;
using System.Security.Claims;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Salhia.KidsLibrary.Application.Common.Interfaces.Security;

/// <summary>
/// Application-facing user gateway that hides ASP.NET Identity and EF Core from Application layer.
/// Implemented in Infrastructure (wrapping UserManager/RoleManager/DbContext).
/// </summary>
public interface IUserService
{

    Task<AppUser> CreateUserAsync(RegisterUserRequest request, CancellationToken ct = default);
    Task<IdentityResult> UpdateUserAsync(UpdateUserRequest user, CancellationToken cancellationToken = default);
    Task<AppUser?> FindByEmailAsync(string email, CancellationToken ct = default);
    Task<AppUser?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByIdAsync(string Id, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByClaimsPrincipalAsync(ClaimsPrincipal user, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetRolesAsync(string userId, CancellationToken ct = default);
    Task<List<string>> GetUserRolesAsync(AppUser user, CancellationToken ct = default);
    Task<bool> AddToRoleAsync(string userId, string roleName, CancellationToken ct = default);
    Task<PagedResult<AppUser>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, Expression<Func<AppUser, object>>[]? includes = null,
  CancellationToken ct = default);
    Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default);
    Task<string?> GenerateEmailConfirmationTokenAsync(string email, CancellationToken ct = default);
    Task<bool> IsEmailConfirmedAsync(string email, CancellationToken ct = default);
    Task<string?> GeneratePasswordResetTokenAsync(string userId, CancellationToken ct = default);
    Task<bool> ValidateCredentialsAsync(string email, string password, bool lockoutOnFailure, CancellationToken ct = default);
    Task<(int UserTypeValue, string UserTypeName)> GetUserTypeAsync(string userId, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken ct = default);
    Task<bool> RemoveFromRoleAsync(string email, string roleName, CancellationToken ct = default);
    Task<bool> UpdateUserTypeAsync(string userId, int userType, CancellationToken ct = default);
    Task<string> NormalizePhoneAsync(string phone, CancellationToken ct);
    Task<AppUser?> FindByPhoneAsync(string phone, CancellationToken ct);
    Task<AppUser> CreateUserAsync(AppUser user, CancellationToken ct);
    Task MarkPhoneConfirmedAsync(string userId, CancellationToken ct);
}
