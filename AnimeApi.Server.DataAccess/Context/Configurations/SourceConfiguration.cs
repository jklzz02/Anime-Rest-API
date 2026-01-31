using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("source");

        entity.HasIndex(e => e.Name, "Name").IsUnique();

        entity.Property(e => e.Name).HasMaxLength(50);
    }
}
