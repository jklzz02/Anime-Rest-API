using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.Business.Extensions.Mapper;

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
            Id = licensorDto.Id,
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