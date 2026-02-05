using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class BanConfiguration : IEntityTypeConfiguration<Ban>
{
    public void Configure(EntityTypeBuilder<Ban> entity)
    {
        entity.ToTable("Ban");

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithMany(u => u.Bans)
            .HasConstraintName("FK_Ban_User");
        
        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamptz")
            .HasColumnName("Created_At");
        
        entity.Property(e => e.Expiration)
            .HasColumnType("timestamptz")
            .HasColumnName("Expiration");

        entity.Property(e => e.NormalizedEmail)
            .HasColumnName("Normalized_Email")
            .HasMaxLength(250)
            .IsRequired();

        entity.Property(e => e.Reason)
            .HasColumnName("Reason")
            .HasMaxLength(250);
    }
}