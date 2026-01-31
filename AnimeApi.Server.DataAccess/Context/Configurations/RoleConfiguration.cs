using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PRIMARY");

        entity.Property(e => e.Access)
            .HasMaxLength(255)
            .IsRequired();

        entity.ToTable("role");
    }
}
