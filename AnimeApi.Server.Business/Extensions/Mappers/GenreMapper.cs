using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="Genre"/> and <see cref="GenreDto"/> objects,
/// as well as collections of these objects.
/// </summary>
public static class GenreMapper
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public static Genre ToModel(this GenreDto genreDto)
    {
        return new Genre
        {
            Id = genreDto.Id ?? 0,
            Name = genreDto.Name
        };
    }

    public static IEnumerable<GenreDto> ToDto(this IEnumerable<Genre> genres)
    {
        return genres.Select(g => g.ToDto());
    }

    public static IEnumerable<Genre> ToModel(this IEnumerable<GenreDto> genres)
    {
        return genres.Select(g => g.ToModel());
    }
}