using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Salhia.KidsLibrary.Infrastructure.Services.Security;

public class JwtService(IConfiguration config) : IJwtService
{
    public Task<string> GenerateTokenAsync(AppUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            claims.Add(new Claim("phone_number", user.PhoneNumber));
            claims.Add(new Claim("phone_number_verified", "true"));
        }

        if (roles != null)
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(encoded);

    }
}
