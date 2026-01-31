using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("refresh_token");

        entity.HasOne(e => e.User)
            .WithOne(e => e.RefreshToken)
            .HasForeignKey<RefreshToken>(e => e.UserId)
            .HasConstraintName("RefreshToken_User_Id_fk")
            .OnDelete(DeleteBehavior.Cascade);

        entity.Property(e => e.HashedToken)
            .IsRequired()
            .HasColumnName("Hashed_Token")
            .HasMaxLength(500);

        entity.Property(e => e.CreatedAt)
            .HasColumnName("Created_At")
            .HasColumnType("timestampz");

        entity.Property(e => e.ExpiresAt)
            .HasColumnName("Expires_At")
            .HasColumnType("timestampz");

        entity.Property(e => e.RevokedAt)
            .HasColumnName("Revoked_At")
            .HasColumnType("timestampz");

        entity.Property(e => e.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        entity.Property(e => e.ExpiresAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        entity.Property(e => e.UserId)
            .HasColumnName("User_Id");

        entity
            .HasIndex(e => e.HashedToken, "Refresh_Token_Hashed_Token__index");
    }
}
