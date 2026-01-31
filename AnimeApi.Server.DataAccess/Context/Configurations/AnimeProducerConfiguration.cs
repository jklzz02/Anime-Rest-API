using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class AnimeProducerConfiguration : IEntityTypeConfiguration<AnimeProducer>
{
    public void Configure(EntityTypeBuilder<AnimeProducer> entity)
    {
        entity.HasKey(e => e.Id);

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
    }
}
