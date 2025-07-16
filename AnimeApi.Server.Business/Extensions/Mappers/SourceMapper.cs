using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="Source"/> and <see cref="SourceDto"/> objects.
/// </summary>
public static class SourceMapper
{
    public static SourceDto ToDto(this Source source)
    {
        return new SourceDto
        {
            Id = source.Id,
            Name = source.Name
        };
    }
    
    public static Source ToModel(this SourceDto source)
    {
        return new Source
        {
            Id = source.Id ?? 0,
            Name = source.Name
        };
    }

    public static IEnumerable<SourceDto> ToDto(this IEnumerable<Source> sources)
    {
        return sources.Select(p => p.ToDto());
    }

    public static IEnumerable<Source> ToModel(this IEnumerable<SourceDto> sources)
    {
        return sources.Select(p => p.ToModel());
    }
}