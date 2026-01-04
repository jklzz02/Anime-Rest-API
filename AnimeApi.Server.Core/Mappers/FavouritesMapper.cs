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
            UserId = entity.UserId,
            AnimeId = entity.AnimeId
        };
    }

    public override Favourite MapToEntity(FavouriteDto dto)
    {
        return new Favourite
        {
            UserId = dto.UserId,
            AnimeId = dto.AnimeId
        };
    }
}