using Salhia.KidsLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Salhia.KidsLibrary.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


   
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


    }
}


