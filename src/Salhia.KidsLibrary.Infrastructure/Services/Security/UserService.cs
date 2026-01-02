using System.Linq.Expressions;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Infrastructure.Services.Security;

public sealed class UserService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signIn
    ) : IUserService
{
    // -------------------- User Commands --------------------

    public async Task<AppUser> CreateUserAsync(RegisterUserRequest request, CancellationToken ct = default)
    {
        // Check if email already exists
        var existingEmail = await userManager.FindByEmailAsync(request.Email);
        if (existingEmail is not null)
            throw new AlreadyExistsException(request.Email, "EmailAlreadyExists");

        // Check if phone number already exists
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var existingPhone = await userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, ct);
            if (existingPhone is not null)
                throw new AlreadyExistsException($"Phone number {request.PhoneNumber}", "PhoneNumberAlreadyExists");
        }

        var user = new AppUser
        {
            Id = Ulid.NewUlid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserType = request.UserType,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return new AppUser { Id = user.Id, Email = user.Email };
    }

    public async Task<IdentityResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User Don't Exist", $"{request.UserId}");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await userManager.UpdateAsync(user);

        return result;
    }

    // -------------------- Queries --------------------
    public async Task<AppUser?> FindByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;

        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;

        return new AppUser
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.Email,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserType = user.UserType,
        };
    }

    public async Task<AppUser?> FindByIdAsync(string Id, CancellationToken cancellationToken = default)
    {
        if (Id == string.Empty) return null;

        var user = await userManager.FindByIdAsync(Id.ToString());
        if (user is null) return null;

        return new AppUser
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.Email,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserType = user.UserType,
        };
    }

    public async Task<AppUser?> GetByIdAsync(string Id, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        IQueryable<AppUser> query = userManager.Users.AsNoTracking();

        // Apply includes if provided
        if (includes?.Length > 0)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);
    }

    public async Task<AppUser?> GetByClaimsPrincipalAsync(ClaimsPrincipal user, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        // First get the user to extract the ID
        var baseUser = await userManager.GetUserAsync(user);
        if (baseUser == null)
            return null;

        // If no includes needed, return the base user
        if (includes?.Length == 0)
            return baseUser;

        // Otherwise, query with includes
        return await GetByIdAsync(baseUser.Id, includes, cancellationToken);
    }

    public async Task<List<string>> GetUserRolesAsync(AppUser user, CancellationToken ct = default)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<PagedResult<AppUser>> GetPagedAsync(
       int pageNumber,
       int pageSize,
       string? search = null,
       UserType? userType = null,
       Expression<Func<AppUser, object>>[]? includes = null,
       CancellationToken ct = default)
    {
        IQueryable<AppUser> query = userManager.Users.AsNoTracking();

        // Apply includes
        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply UserType filter
        if (userType.HasValue)
        {
            query = query.Where(u => u.UserType == userType.Value);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u =>
                    (u.Email ?? string.Empty).ToLower().Contains(search) ||
                    (u.FirstName ?? string.Empty).ToLower().Contains(search) ||
                    (u.LastName ?? string.Empty).ToLower().Contains(search) || 
                    (u.PhoneNumber ?? string.Empty).ToLower().Contains(search)
                );
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(ct);

        return new PagedResult<AppUser>(items, total, pageSize, pageNumber);
    }


    // -------------------- Roles --------------------
    public async Task<IReadOnlyList<string>> GetRolesAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return Array.Empty<string>();
        var roles = await userManager.GetRolesAsync(user);
        return roles.ToArray();
    }
    public async Task<bool> AddToRoleAsync(string userId, string roleName, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var res = await userManager.AddToRoleAsync(user, roleName);
        return res.Succeeded;
    }
    public async Task<bool> RemoveFromRoleAsync(string email, string roleName, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var res = await userManager.RemoveFromRoleAsync(user, roleName);
        return res.Succeeded;
    }

    public async Task<bool> UpdateUserTypeAsync(string userId, int userType, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        
        user.UserType = (Domain.Enums.UserType)userType;
        user.UpdatedAt = DateTime.UtcNow;
        
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DisableUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        
        await userManager.SetLockoutEnabledAsync(user, true);
        var result = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        return result.Succeeded;
    }

    public async Task<bool> EnableUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        
        var result = await userManager.SetLockoutEndDateAsync(user, null);
        return result.Succeeded;
    }

    public async Task<bool> IsUserDisabledAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        return await userManager.IsLockedOutAsync(user);
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        
        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<List<AppUser>> GetAllAdminsAsync(CancellationToken ct = default)
    {
        var admins = await userManager.GetUsersInRoleAsync(UserRole.Admin.ToString());
        return admins.ToList();
    }

    // -------- Auth / credentials --------
    public async Task<SignInResult> ValidateCredentialsAsync(string email, string password, bool lockoutOnFailure, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return SignInResult.Failed;
        var result = await signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        return result;
    }

    // -------- User type --------
    public async Task<(int UserTypeValue, string UserTypeName)> GetUserTypeAsync(string userId, CancellationToken ct = default)
    {

        var user = await userManager.FindByIdAsync(userId.ToString());

        var roles = await GetRolesAsync(userId, ct);

        return ((int)user!.UserType, user.UserType.ToString());
    }

    // -------------------- Email Confirmation (by Id) --------------------
    public async Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string?> GenerateEmailConfirmationTokenAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> IsEmailConfirmedAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        return await userManager.IsEmailConfirmedAsync(user);
    }

    // -------------------- Email Confirmation (by Email) --------------------
    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }


    // -------------------- Password Reset --------------------
    public async Task<string?> GeneratePasswordResetTokenAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public Task<string> NormalizePhoneAsync(string phone, CancellationToken ct)
    {
        // TODO: use libphonenumber for true E.164; here�s a simple fallback
        var p = phone.Trim().Replace(" ", "");
        return Task.FromResult(p);
    }

    public Task<AppUser?> FindByPhoneAsync(string phone, CancellationToken ct)
        => userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone, ct);

    public async Task<AppUser> CreateUserAsync(AppUser user, CancellationToken ct)
    {
        // Create without password (passwordless / OTP-based)
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return user;
    }

    public async Task MarkPhoneConfirmedAsync(string userId, CancellationToken ct)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId, ct);
        if (!user.PhoneNumberConfirmed)
        {
            user.PhoneNumberConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }
    }
}
