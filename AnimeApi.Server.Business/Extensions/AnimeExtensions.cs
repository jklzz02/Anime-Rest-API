
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Extensions;

public static class AnimeExtensions
{
    public static AnimeSummary ToSummary(this AnimeDto entity)
    {
        return new AnimeSummary
        {
            Id = entity.Id ?? default,
            Name = entity.Name,
            ImageUrl = entity.ImageUrl,
            Score = entity.Score,
            ReleaseYear = entity.ReleaseYear,
            Rating = entity.Rating
        };
    }

    public static IEnumerable<AnimeSummary> ToSummary(this IEnumerable<AnimeDto> entities)
    {
        return entities.Select(e => e.ToSummary());
    }
}