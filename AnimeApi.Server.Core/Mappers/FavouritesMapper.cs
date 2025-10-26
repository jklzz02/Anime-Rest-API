using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Mappers;

public class FavouritesMapper : Mapper<Favourite, FavouriteDto>
{
    public override FavouriteDto MapToDto(Favourite entity)
    {
        return new FavouriteDto
        {
            UserId = entity.User_Id,
            AnimeId = entity.Anime_Id
        };
    }

    public override Favourite MapToEntity(FavouriteDto dto)
    {
        return new Favourite
        {
            User_Id = dto.UserId,
            Anime_Id = dto.AnimeId
        };
    }
}