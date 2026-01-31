using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class AnimeGenreConfiguration : IEntityTypeConfiguration<AnimeGenre>
{
    public void Configure(EntityTypeBuilder<AnimeGenre> entity)
    {
        entity.HasKey(e => e.Id);

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
    }
}
