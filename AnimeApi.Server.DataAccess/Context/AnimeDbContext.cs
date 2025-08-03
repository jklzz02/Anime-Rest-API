﻿using AnimeApi.Server.Core.Objects.Models;
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

    public virtual DbSet<AnimeGenre> Anime_Genres { get; set; }

    public virtual DbSet<AnimeLicensor> Anime_Licensors { get; set; }

    public virtual DbSet<AnimeProducer> Anime_Producers { get; set; }
    
    public virtual DbSet<AppUser> Users { get; set; }
    
    public virtual DbSet<Favourite> User_Favourites { get; set; }
    
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
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime");

            entity.HasIndex(e => e.English_Name, "Anime_English_Name_index");

            entity.HasIndex(e => e.Episodes, "Anime_Episodes_index");

            entity.HasIndex(e => e.Name, "Anime_Name_index");

            entity.HasIndex(e => e.Release_Year, "Anime_Release_Year_index");

            entity.HasIndex(e => e.Score, "Anime_Score_index");

            entity.HasIndex(e => e.SourceId, "Anime_Source_Id_fk");

            entity.HasIndex(e => e.TypeId, "Anime_Type_Id_fk");

            entity.Property(e => e.Background).HasMaxLength(1000);
            entity.Property(e => e.Duration).HasMaxLength(255);
            entity.Property(e => e.Image_URL).HasMaxLength(255);
            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Other_Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Rating).HasMaxLength(100);
            entity.Property(e => e.Score).HasPrecision(3, 1);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Studio)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Trailer_embed_url).HasMaxLength(255);
            entity.Property(e => e.Trailer_image_url).HasMaxLength(255);
            entity.Property(e => e.Trailer_url).HasMaxLength(255);

            entity.HasOne(d => d.Source).WithMany(p => p.Animes)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Anime_Source_Id_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Animes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Type_Id_fk");
        });

        modelBuilder.Entity<AnimeGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Genre");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.GenreId, "GenreId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Genres)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Genre_ibfk_1");

            entity.HasOne(d => d.Genre).WithMany(p => p.Anime_Genres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Anime_Genre_ibfk_2");
        });
        
        modelBuilder.Entity<AnimeGenre>()
            .Navigation(ag => ag.Genre)
            .AutoInclude();

        modelBuilder.Entity<AnimeLicensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Licensor");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.LicensorId, "LicensorId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Licensors)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Licensor_ibfk_1");

            entity.HasOne(d => d.Licensor).WithMany(p => p.Anime_Licensors)
                .HasForeignKey(d => d.LicensorId)
                .HasConstraintName("Anime_Licensor_ibfk_2");
        });
        
        modelBuilder.Entity<AnimeLicensor>()
            .Navigation(al => al.Licensor)
            .AutoInclude();

        modelBuilder.Entity<AnimeProducer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Producer");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.ProducerId, "ProducerId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Producers)
                .HasForeignKey(d => d.AnimeId)
                .HasConstraintName("Anime_Producer_ibfk_1");

            entity.HasOne(d => d.Producer).WithMany(p => p.Anime_Producers)
                .HasForeignKey(d => d.ProducerId)
                .HasConstraintName("Anime_Producer_ibfk_2");
        });
        
        modelBuilder.Entity<AnimeProducer>()
            .Navigation(al => al.Producer)
            .AutoInclude();

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Genre");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "Email")
                .IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255);

            entity.Property(e => e.Username)
                .HasMaxLength(255);

            entity.Property(e => e.Picture_Url)
                .HasMaxLength(255);

            entity.HasOne<Role>(e => e.Role).WithMany(r => r.Users)
                .HasForeignKey(e => e.Role_Id)
                .HasConstraintName("User_Role_Id_fk");

            entity.HasOne<RefreshToken>(e => e.RefreshToken)
                .WithOne(e => e.User);
        });
        
        modelBuilder.Entity<AppUser>()
            .Navigation(u => u.Role)
            .AutoInclude();

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => new { e.User_Id, e.Anime_Id })
                .HasName("PRIMARY");;
            
            entity.HasOne(e => e.User).WithMany(u => u.Favourites)
                .HasForeignKey(e => e.User_Id)
                .HasConstraintName("User_Favourites_User_Id_fk");
            
            entity.HasOne(e => e.Anime).WithMany(a => a.Favourites)
                .HasForeignKey(e => e.Anime_Id)
                .HasConstraintName("User_Favourites_Anime_Id_fk");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("Refresh_Token");
            
            entity.HasOne(e => e.User)
                .WithOne(e => e.RefreshToken)
                .HasForeignKey<RefreshToken>(e => e.User_Id)
                .HasConstraintName("RefreshToken_User_Id_fk")
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(e => e.Hashed_Token)
                .IsRequired()
                .HasMaxLength(500);

            entity
                .HasIndex(e => e.Hashed_Token, "Refresh_Token_Hashed_Token__index");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.ToTable("Review");
            
            entity.Property(e => e.Content)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3")
                .IsRequired();
            
            entity.HasOne(r => r.Anime).WithMany(a => a.Reviews)
                .HasForeignKey(r => r.Anime_Id)
                .HasConstraintName("Anime_Id_fk");
            
            entity.HasOne(r => r.User).WithMany(u => u.Reviews)
                .HasForeignKey(r => r.User_Id)
                .HasConstraintName("User_Id_Fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");
            
            entity.Property(e => e.Access)
                .HasMaxLength(255)
                .IsRequired();
            
            entity.ToTable("Role");
        });

        modelBuilder.Entity<Licensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Licensor");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Producer");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Source");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TypeModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Type");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
