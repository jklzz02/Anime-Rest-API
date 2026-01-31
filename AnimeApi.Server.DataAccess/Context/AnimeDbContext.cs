using AnimeApi.Server.Core.Objects.Models;
using TypeModel = AnimeApi.Server.Core.Objects.Models.Type;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Context;

public partial class AnimeDbContext : DbContext
{
    public AnimeDbContext()
    {
    }

    public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Anime { get; set; }

    public virtual DbSet<AnimeGenre> AnimeGenres { get; set; }

    public virtual DbSet<AnimeLicensor> AnimeLicensors { get; set; }

    public virtual DbSet<AnimeProducer> AnimeProducers { get; set; }
    
    public virtual DbSet<AppUser> Users { get; set; }
    
    public virtual DbSet<Favourite> UserFavourites { get; set; }
    
    public virtual DbSet<Review> Reviews { get; set; }
    
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Licensor> Licensors { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<TypeModel> Types { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from the Configurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimeDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
