
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.Business.Mappers;
public interface IAnimeMapper : IMapper<Anime, AnimeDto>
{
    Anime MapToEntity(AnimeDto dto, bool includeNavigation);

    IEnumerable<Anime> MapToEntity(IEnumerable<AnimeDto> dto, bool includeNavigation);
}
