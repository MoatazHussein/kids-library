using Salhia.KidsLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Salhia.KidsLibrary.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<CustomStory> CustomStories { get; set; }
    public DbSet<CustomStoryItem> CustomStoryItems { get; set; }
    public DbSet<StoryCategory> StoryCategories { get; set; }
    public DbSet<MasterStory> MasterStories { get; set; }
    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<StoryComment> StoryComments { get; set; }
    public DbSet<FavoriteStory> FavoriteStories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


    }
}


