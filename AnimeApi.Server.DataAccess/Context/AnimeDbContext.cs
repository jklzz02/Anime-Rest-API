using System;
using System.Collections.Generic;
using AnimeApi.Server.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Context;

public partial class AnimeDbContext : DbContext
{
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime");

            entity.Property(e => e.Duration).HasMaxLength(255);
            entity.Property(e => e.English_Name).HasMaxLength(255);
            entity.Property(e => e.Image_URL).HasMaxLength(255);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Other_Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Rating).HasMaxLength(100);
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Studio)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type).HasColumnType("enum('TV','Movie','OVA','ONA','Special','Music','UNKNOWN')");
        });

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
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Studio)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(5000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type).HasColumnType("enum('TV','Movie','OVA','ONA','Special','Music','UNKNOWN')");
        });

        modelBuilder.Entity<Anime_Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Genre");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.GenreId, "GenreId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Genres)
                .HasForeignKey(d => d.AnimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Genre_ibfk_1");

            entity.HasOne(d => d.Genre).WithMany(p => p.Anime_Genres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Genre_ibfk_2");
        });

        modelBuilder.Entity<Anime_Licensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Licensor");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.LicensorId, "LicensorId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Licensors)
                .HasForeignKey(d => d.AnimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Licensor_ibfk_1");

            entity.HasOne(d => d.Licensor).WithMany(p => p.Anime_Licensors)
                .HasForeignKey(d => d.LicensorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Licensor_ibfk_2");
        });

        modelBuilder.Entity<Anime_Producer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Anime_Producer");

            entity.HasIndex(e => e.AnimeId, "AnimeId");

            entity.HasIndex(e => e.ProducerId, "ProducerId");

            entity.HasOne(d => d.Anime).WithMany(p => p.Anime_Producers)
                .HasForeignKey(d => d.AnimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Producer_ibfk_1");

            entity.HasOne(d => d.Producer).WithMany(p => p.Anime_Producers)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Anime_Producer_ibfk_2");
        });

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
