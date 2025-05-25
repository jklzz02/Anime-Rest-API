using AnimeApi.Server.Business.Dto;
using Type = AnimeApi.Server.DataAccess.Models.Type;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides extension methods for mapping between the <see cref="Type"/> model and <see cref="TypeDto"/>.
/// </summary>
public static class TypeMapper
{
    public static TypeDto ToDto(this Type type)
    {
        return new TypeDto
        {
            Id = type.Id,
            Name = type.Name
        };
    }
    
    public static Type ToModel(this TypeDto type)
    {
        return new Type
        {
            Id = type.Id ?? 0,
            Name = type.Name
        };
    }

    public static IEnumerable<TypeDto> ToDto(this IEnumerable<Type> types)
    {
        return types.Select(p => p.ToDto());
    }

    public static IEnumerable<Type> ToModel(this IEnumerable<TypeDto> types)
    {
        return types.Select(p => p.ToModel());
    }
}