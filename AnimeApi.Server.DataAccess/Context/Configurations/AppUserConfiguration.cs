using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("user");

        entity.HasIndex(e => e.Email, "Email")
            .IsUnique();

        entity.Property(e => e.Email)
            .HasMaxLength(255);

        entity.Property(e => e.Username)
            .HasMaxLength(255);

        entity.Property(e => e.PictureUrl)
            .HasColumnName("Picture_Url")
            .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
            .HasColumnName("Created_At")
            .HasConversion(
                c => c.ToUniversalTime(),
                c => DateTime.SpecifyKind(c, DateTimeKind.Utc))
            .HasColumnType("timestampz");

        entity.Property(e => e.RoleId)
            .HasColumnName("Role_Id");

        entity.HasOne<Role>(e => e.Role).WithMany(r => r.Users)
            .HasForeignKey(e => e.RoleId)
            .HasConstraintName("User_Role_Id_fk");

        entity.HasOne(e => e.RefreshToken)
            .WithOne(e => e.User);

        entity.Navigation(u => u.Role)
            .AutoInclude();
    }
}
