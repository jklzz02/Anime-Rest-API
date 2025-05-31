using System;
using System.Collections.Generic;
using AnimeApi.Server.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Type = AnimeApi.Server.DataAccess.Models.Type;

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

    public virtual DbSet<AnimeFullInfo> AnimeFullInfo { get; set; }

    public virtual DbSet<Anime_Genre> Anime_Genres { get; set; }

    public virtual DbSet<Anime_Licensor> Anime_Licensors { get; set; }

    public virtual DbSet<Anime_Producer> Anime_Producers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Licensor> Licensors { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<Type> Types { get; set; }
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
            
            entity.Property(e => e.Background)
                .HasMaxLength(1000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            
            entity.Property(e => e.Trailer_image_url)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            
            entity.Property(e => e.Trailer_url)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            
            entity.Property(e => e.Trailer_embed_url)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.Source).WithMany(p => p.Animes)
                .HasForeignKey(d => d.SourceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Anime_Source_Id_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Animes)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Type_Id_fk");
        });
        
        modelBuilder.Entity<Anime>()
            .Navigation(a => a.Anime_Genres)
            .AutoInclude();

        modelBuilder.Entity<Anime>()
            .Navigation(a => a.Anime_Licensors)
            .AutoInclude();

        modelBuilder.Entity<Anime>()
            .Navigation(a => a.Anime_Producers)
            .AutoInclude();

        modelBuilder.Entity<Anime>()
            .Navigation(a => a.Type)
            .AutoInclude();
        
        modelBuilder.Entity<Anime>()
            .Navigation(a => a.Source)
            .AutoInclude();

        modelBuilder.Entity<AnimeFullInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AnimeFullInfo");

            entity.Property(e => e.Duration).HasMaxLength(255);
            entity.Property(e => e.English_Name).HasMaxLength(255);
            entity.Property(e => e.Genres).HasColumnType("text");
            entity.Property(e => e.Image_URL).HasMaxLength(255);
            entity.Property(e => e.Licensors)
                .HasColumnType("text")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Other_Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Producers)
                .HasColumnType("text")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Rating).HasMaxLength(100);
            entity.Property(e => e.Score).HasPrecision(3, 1);
            entity.Property(e => e.Source).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Studio)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Anime_Genre>(entity =>
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
        
        modelBuilder.Entity<Anime_Genre>()
            .Navigation(ag => ag.Genre)
            .AutoInclude();

        modelBuilder.Entity<Anime_Licensor>(entity =>
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
        
        modelBuilder.Entity<Anime_Licensor>()
            .Navigation(al => al.Licensor)
            .AutoInclude();

        modelBuilder.Entity<Anime_Producer>(entity =>
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
        
        modelBuilder.Entity<Anime_Producer>()
            .Navigation(al => al.Producer)
            .AutoInclude();

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Genre");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
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

        modelBuilder.Entity<Type>(entity =>
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
