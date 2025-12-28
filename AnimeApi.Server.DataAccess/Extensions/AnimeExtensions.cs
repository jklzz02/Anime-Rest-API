using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.DataAccess.Extensions;

public static class AnimeExtensions
{
    public static AnimeSummary ToSummary(this Anime entity)
    {
        return new AnimeSummary
        {
            Id = entity.Id,
            Name = entity.Name,
            Image_URL = entity.Image_URL,
            Score = entity.Score,
            Release_Year = entity.Release_Year,
            Rating = entity.Rating
        };
    }

    public static IEnumerable<AnimeSummary> ToSummary(this IEnumerable<Anime> entities)
    {
        return entities.Select(e => e.ToSummary());
    }
}