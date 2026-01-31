using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeApi.Server.DataAccess.Context.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> entity)
    {
        entity.HasKey(e => new { User_Id = e.UserId, Anime_Id = e.AnimeId });

        entity.ToTable("user_favourites");

        entity.HasOne(e => e.User).WithMany(u => u.Favourites)
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("User_Favourites_User_Id_fk");

        entity.HasOne(e => e.Anime).WithMany(a => a.Favourites)
            .HasForeignKey(e => e.AnimeId)
            .HasConstraintName("User_Favourites_Anime_Id_fk");

        entity.Property(e => e.AnimeId)
            .HasColumnName("Anime_Id");

        entity.Property(e => e.UserId)
            .HasColumnName("User_Id");
    }
}
