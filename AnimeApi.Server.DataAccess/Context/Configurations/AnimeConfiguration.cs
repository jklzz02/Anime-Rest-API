using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
{
    public void Configure(EntityTypeBuilder<Anime> entity)
    {
        entity.HasKey(e => e.Id);

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
        entity.Property(e => e.Name);

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
            .HasMaxLength(255);

        entity.Property(e => e.OtherName)
            .HasColumnName("Other_Name")
            .HasMaxLength(255);

        entity.Property(e => e.Rating).HasMaxLength(100);
        entity.Property(e => e.Score).HasPrecision(3, 1);
        entity.Property(e => e.Status).HasMaxLength(50);
        entity.Property(e => e.Studio)
            .HasMaxLength(255);
        entity.Property(e => e.Synopsis)
            .HasMaxLength(5000);

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
    }
}
