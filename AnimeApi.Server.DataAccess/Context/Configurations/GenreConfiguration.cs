using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> entity)
    {
        entity.HasKey(e => e.Id).HasName("PRIMARY");

        entity.ToTable("genre");

        entity.HasIndex(e => e.Name, "Name").IsUnique();

        entity.Property(e => e.Name).HasMaxLength(50);
    }
}
