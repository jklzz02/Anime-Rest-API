using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="Licensor"/> and <see cref="LicensorDto"/> objects.
/// </summary>
public static class LicensorMapper
{
    public static LicensorDto ToDto(this Licensor licensor)
    {
        return new LicensorDto
        {
            Id = licensor.Id,
            Name = licensor.Name
        };
    }
    
    public static Licensor ToModel(this LicensorDto licensorDto)
    {
        return new Licensor
        {
            Id = licensorDto.Id ?? 0,
            Name = licensorDto.Name
        };
    }

    public static IEnumerable<LicensorDto> ToDto(this IEnumerable<Licensor> licensors)
    {
        return licensors.Select(s => s.ToDto());
    }    
    
    public static IEnumerable<Licensor> ToDto(this IEnumerable<LicensorDto> licensors)
    {
        return licensors.Select(s => s.ToModel());
    }
}