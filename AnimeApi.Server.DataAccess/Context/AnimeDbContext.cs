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
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci");
        
        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anime");

            entity.HasIndex(e => e.EnglishName, "Anime_English_Name_index");

            entity.HasIndex(e => e.Episodes, "Anime_Episodes_index");

            entity.HasIndex(e => e.Name, "Anime_Name_index");

            entity.HasIndex(e => e.ReleaseYear, "Anime_Release_Year_index");

            entity.HasIndex(e => e.Score, "Anime_Score_index");

            entity.HasIndex(e => e.SourceId, "Anime_Source_Id_fk");

            entity.HasIndex(e => e.TypeId, "Anime_Type_Id_fk");

            entity.Property(e => e.Background).HasMaxLength(1000);
            entity.Property(e => e.Duration).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.ImageUrl)
                .HasColumnName("Image_URL");
            
            entity.Property(e => e.ReleaseYear)
                .HasColumnName("Release_Year");
            
            entity.Property(e => e.StartedAiring)
                .HasColumnName("Started_Airing")
                .HasColumnType("timestamptz");
            
            entity.Property(e => e.FinishedAiring)
                .HasColumnName("Finished_Airing")
                .HasColumnType("timestamptz");
            
            entity.Property(e => e.EnglishName)
                .HasColumnName("English_Name")
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci");
            
            entity.Property(e => e.OtherName)
                .HasColumnName("Other_Name")
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci");
            
            entity.Property(e => e.Rating).HasMaxLength(100);
            entity.Property(e => e.Score).HasPrecision(3, 1);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Studio)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci");
            
            entity.Property(e => e.TrailerEmbedUrl)
                .HasColumnName("Trailer_embed_url")
                .HasMaxLength(255);
            entity.Property(e => e.TrailerImageUrl)
                .HasColumnName("Trailer_image_url")
                .HasMaxLength(255);
            entity.Property(e => e.TrailerUrl)
                .HasColumnName("Trailer_url")
                .HasMaxLength(255);

            entity.HasOne(d => d.Source).WithMany(p => p.Anime)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Anime_Source_Id_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Anime)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Anime_Type_Id_fk");
        });

        modelBuilder.Entity<AnimeGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anime_genre");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.GenreId, "GenreId");

            entity.HasOne(d => d.Anime).WithMany(p => p.AnimeGenres)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Genre_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Genre).WithMany(p => p.AnimeGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Anime_Genre_ibfk_2")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AnimeLicensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anime_licensor");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.LicensorId, "LicensorId");

            entity.HasOne(d => d.Anime).WithMany(p => p.AnimeLicensors)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Licensor_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Licensor).WithMany(p => p.AnimeLicensors)
                .HasForeignKey(d => d.LicensorId)
                .HasConstraintName("Anime_Licensor_ibfk_2")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AnimeProducer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("anime_producer");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.ProducerId, "ProducerId");

            entity.HasOne(d => d.Anime).WithMany(p => p.AnimeProducers)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Producer_ibfk_1")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Producer).WithMany(p => p.AnimeProducers)
                .HasForeignKey(d => d.ProducerId)
                .HasConstraintName("Anime_Producer_ibfk_2")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genre");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "Email")
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255);

            entity.Property(e => e.Username)
                .HasMaxLength(255);

            entity.Property(e => e.PictureUrl)
                .HasColumnName("Picture_Url")
                .HasMaxLength(255);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("Created_At")
                .HasConversion(
                    c => c.ToUniversalTime(),
                    c => DateTime.SpecifyKind(c, DateTimeKind.Utc))
                .HasColumnType("timestampz");

            entity.Property(e => e.RoleId)
                .HasColumnName("Role_Id");
            
            entity.HasOne<Role>(e => e.Role).WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("User_Role_Id_fk");
            
            entity.HasOne(e => e.RefreshToken)
                .WithOne(e => e.User);
        });
        
        modelBuilder.Entity<AppUser>()
            .Navigation(u => u.Role)
            .AutoInclude();

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => new { User_Id = e.UserId, Anime_Id = e.AnimeId })
                .HasName("PRIMARY");;
            
            entity.ToTable("user_favourites");
            
            entity.HasOne(e => e.User).WithMany(u => u.Favourites)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("User_Favourites_User_Id_fk");
            
            entity.HasOne(e => e.Anime).WithMany(a => a.Favourites)
                .HasForeignKey(e => e.AnimeId)
                .HasConstraintName("User_Favourites_Anime_Id_fk");

            entity.Property(e => e.AnimeId)
                .HasColumnName("Anime_Id");
            
            entity.Property(e => e.UserId)
                .HasColumnName("User_Id");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("refresh_token");
            
            entity.HasOne(e => e.User)
                .WithOne(e => e.RefreshToken)
                .HasForeignKey<RefreshToken>(e => e.UserId)
                .HasConstraintName("RefreshToken_User_Id_fk")
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(e => e.HashedToken)
                .IsRequired()
                .HasColumnName("Hashed_Token")
                .HasMaxLength(500);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("Created_At")
                .HasColumnType("timestampz");
            
            entity.Property(e => e.ExpiresAt)
                .HasColumnName("Expires_At")
                .HasColumnType("timestampz");
            
            entity.Property(e => e.RevokedAt)
                .HasColumnName("Revoked_At")
                .HasColumnType("timestampz");

            entity.Property(e => e.CreatedAt)
                .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            entity.Property(e => e.ExpiresAt)
                .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
            
            entity.Property(e => e.UserId)
                .HasColumnName("User_Id");

            entity
                .HasIndex(e => e.HashedToken, "Refresh_Token_Hashed_Token__index");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("review");

            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .UseCollation("utf8mb3_general_ci")
                .IsRequired();
            
            entity.Property(e => e.Content)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .IsRequired();
            
            entity.Property(e => e.AnimeId)
                .HasColumnName("Anime_Id");
            
            entity.Property(e => e.UserId)
                .HasColumnName("User_Id");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("Created_At");
            
            entity.HasOne(r => r.Anime).WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AnimeId)
                .HasConstraintName("Anime_Id_fk");
            
            entity.HasOne(r => r.User).WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .HasConstraintName("User_Id_Fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.Property(e => e.Access)
                .HasMaxLength(255)
                .IsRequired();
            
            entity.ToTable("role");
        });

        modelBuilder.Entity<Licensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("licensor");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producer");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci");
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("source");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TypeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("type");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
