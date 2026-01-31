using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class LicensorConfiguration : IEntityTypeConfiguration<Licensor>
{
    public void Configure(EntityTypeBuilder<Licensor> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("licensor");

        entity.HasIndex(e => e.Name, "Name").IsUnique();

        entity.Property(e => e.Name);
    }
}
