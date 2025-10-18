using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Mappers;

public class AnimeSummaryMapper
{
    public AnimeSummaryDto MapToDto(AnimeSummary animeSummary)
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

    public AnimeSummary MapToEntityl(AnimeSummaryDto animeSummaryDto)
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

    public IEnumerable<AnimeSummaryDto> MapToDto(IEnumerable<AnimeSummary> summaries)
        => summaries.Select(MapToDto);
    
    public IEnumerable<AnimeSummary> MapToModel(IEnumerable<AnimeSummaryDto> summaries)
        => summaries.Select(MapToEntityl);
}