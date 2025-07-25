using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public static class AnimeSummaryMapper
{
    public static AnimeSummaryDto ToDto(this AnimeSummary animeSummary)
    {
        return new AnimeSummaryDto
        {
            Id = animeSummary.Id,
            Name = animeSummary.Name,
            ImageUrl = animeSummary.ImageUrl,
            Score = animeSummary.Score,
            ReleaseYear = animeSummary.ReleaseYear,
            Rating = animeSummary.Rating,
        };
    }

    public static AnimeSummary ToModel(this AnimeSummaryDto animeSummaryDto)
    {
        return new AnimeSummary
        {
            Id = animeSummaryDto.Id,
            Name = animeSummaryDto.Name,
            ImageUrl = animeSummaryDto.ImageUrl,
            Score = animeSummaryDto.Score,
            ReleaseYear = animeSummaryDto.ReleaseYear,
            Rating = animeSummaryDto.Rating,
        };
    }

    public static IEnumerable<AnimeSummaryDto> ToDto(this IEnumerable<AnimeSummary> summaries)
    {
        return summaries.Select(s => s.ToDto());
    }
    
    public static IEnumerable<AnimeSummary> ToModel(this IEnumerable<AnimeSummaryDto> summaries)
    {
        return summaries.Select(s => s.ToModel());
    }
}