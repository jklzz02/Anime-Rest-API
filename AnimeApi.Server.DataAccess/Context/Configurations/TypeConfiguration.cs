using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TypeModel = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class TypeConfiguration : IEntityTypeConfiguration<TypeModel>
{
    public void Configure(EntityTypeBuilder<TypeModel> entity)
    {
        entity.HasKey(e => e.Id).HasName("PRIMARY");

        entity.ToTable("type");

        entity.HasIndex(e => e.Name, "Name").IsUnique();

        entity.Property(e => e.Name).HasMaxLength(50);
    }
}
