using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PRIMARY");

        entity.ToTable("review");

        entity.Property(e => e.Title)
            .HasMaxLength(30)
            .IsRequired();

        entity.Property(e => e.Content)
            .HasMaxLength(5000)
            .IsRequired();

        entity.Property(e => e.AnimeId)
            .HasColumnName("Anime_Id");

        entity.Property(e => e.UserId)
            .HasColumnName("User_Id");

        entity.Property(e => e.CreatedAt)
            .HasColumnName("Created_At");

        entity.HasOne(r => r.Anime).WithMany(a => a.Reviews)
            .HasForeignKey(r => r.AnimeId)
            .HasConstraintName("Anime_Id_fk");

        entity.HasOne(r => r.User).WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .HasConstraintName("User_Id_Fk");
    }
}
