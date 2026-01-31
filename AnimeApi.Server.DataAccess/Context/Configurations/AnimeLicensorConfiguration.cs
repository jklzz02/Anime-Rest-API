using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class AnimeLicensorConfiguration : IEntityTypeConfiguration<AnimeLicensor>
{
    public void Configure(EntityTypeBuilder<AnimeLicensor> entity)
    {
        entity.HasKey(e => e.Id);

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
    }
}
