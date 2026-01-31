using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("producer");

        entity.HasIndex(e => e.Name, "Name").IsUnique();

        entity.Property(e => e.Name);
    }
}
