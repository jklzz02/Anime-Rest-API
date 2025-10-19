using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public class FavouritesMapper : IMapper<Favourite, FavouriteDto>
{
    public FavouriteDto MapToDto(Favourite entity)
    {
        return new FavouriteDto
        {
            UserId = entity.User_Id,
            AnimeId = entity.Anime_Id
        };
    }

    public Favourite MapToEntity(FavouriteDto dto)
    {
        return new Favourite
        {
            User_Id = dto.UserId,
            Anime_Id = dto.AnimeId
        };
    }

    public IEnumerable<FavouriteDto> MapToDto( IEnumerable<Favourite> entities)
    {
        return entities.Select(MapToDto);
    }

    public IEnumerable<Favourite> MapToEntity(IEnumerable<FavouriteDto> dto)
    {
        return dto.Select(MapToEntity);
    }
}