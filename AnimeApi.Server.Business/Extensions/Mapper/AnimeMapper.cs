using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.Business.Extensions.Mapper;

public static class AnimeMapper
{
    public static Anime ToModel(this AnimeDto dto)
    {
        return new Anime
        {
            Id = dto.Id,
            Name = dto.Name,
            English_Name = dto.EnglishName,
            Other_Name = dto.OtherName,
            Synopsis = dto.Synopsis,
            Image_URL = dto.ImageUrl,
            Type = dto.Type,
            Episodes = dto.Episodes,
            Duration = dto.Duration,
            Source = dto.Source,
            Release_Year = dto.ReleaseYear,
            Started_Airing = dto.StartedAiring,
            Finished_Airing = dto.FinishedAiring,
            Rating = dto.Rating,
            Studio = dto.Studio,
            Score = dto.Score,
            Status = dto.Status,

            Anime_Genres = dto.Genres?
                .Select(g => new Anime_Genre { GenreId = g.Id })
                .ToList() ?? new List<Anime_Genre>(),

            Anime_Producers = dto.Producers?
                .Select(p => new Anime_Producer { ProducerId = p.Id })
                .ToList() ?? new List<Anime_Producer>(),

            Anime_Licensors = dto.Licensors?
                .Select(l => new Anime_Licensor { LicensorId = l.Id })
                .ToList() ?? new List<Anime_Licensor>()
        };
    }
    
    public static AnimeDto ToDto(this Anime anime)
    {
        return new AnimeDto()
        {
            Id = anime.Id,
            Name = anime.Name,
            EnglishName = anime.English_Name,
            OtherName = anime.Other_Name,
            Synopsis = anime.Synopsis,
            ImageUrl = anime.Image_URL,
            Type = anime.Type,
            Episodes = anime.Episodes,
            Duration = anime.Duration,
            Source = anime.Source,
            ReleaseYear = anime.Release_Year,
            StartedAiring = anime.Started_Airing,
            FinishedAiring = anime.Finished_Airing,
            Rating = anime.Rating,
            Studio = anime.Studio,
            Score = anime.Score,
            Status = anime.Status,
            
            Genres = anime.Anime_Genres
                .Select(ag => ag.Genre)
                .Select(g => new GenreDto() {Id = g.Id, Name = g.Name})
                .ToList(),
            
            Producers = anime.Anime_Producers
                .Select(ap => ap.Producer)
                .Select(p => new ProducerDto() {Id = p.Id, Name = p.Name})
                .ToList(),
            
            Licensors = anime.Anime_Licensors
                .Select(al => al.Licensor)
                .Select(l => new LicensorDto() {Id = l.Id, Name = l.Name})
                .ToList()
        };
    }

    public static IEnumerable<AnimeDto> ToDto(this IEnumerable<Anime> anime)
    {
        return anime.Select(a => a.ToDto());
    }

    public static IEnumerable<Anime> ToModel(this IEnumerable<AnimeDto> anime)
    {
        return anime.Select(a => a.ToModel());
    }
}