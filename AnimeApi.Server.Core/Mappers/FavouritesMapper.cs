using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public static class FavouritesMapper
{
    public static FavouriteDto ToDto(this Favourite entity)
    {
        return new FavouriteDto
        {
            UserId = entity.User_Id,
            AnimeId = entity.Anime_Id
        };
    }

    public static Favourite ToModel(this FavouriteDto dto)
    {
        return new Favourite
        {
            User_Id = dto.UserId,
            Anime_Id = dto.AnimeId
        };
    }

    public static IEnumerable<FavouriteDto> ToDto(this IEnumerable<Favourite> entities)
    {
        return entities.Select(e => e.ToDto());
    }

    public static IEnumerable<Favourite> ToModel(this IEnumerable<FavouriteDto> dto)
    {
        return dto.Select(d => d.ToModel());
    }
}