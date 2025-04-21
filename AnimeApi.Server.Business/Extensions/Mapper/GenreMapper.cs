using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.Business.Extensions.Mapper;

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
            Id = genreDto.Id,
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