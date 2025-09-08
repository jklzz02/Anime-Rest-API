using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Business.Extensions.Mappers;

/// <summary>
/// Provides mapping extension methods for converting between <see cref="Anime"/> and <see cref="AnimeDto"/> objects.
/// </summary>
public static class AnimeMapper
{
    public static Anime ToModel(this AnimeDto dto, bool includeNavigation = true)
    {
        var entity =  new Anime
        {
            Id = dto.Id ?? 0,
            Name = dto.Name,
            English_Name = dto.EnglishName,
            Other_Name = dto.OtherName,
            Synopsis = dto.Synopsis,
            Image_URL = dto.ImageUrl,
            TypeId = dto.Type?.Id ?? 0,
            Episodes = dto.Episodes,
            Duration = dto.Duration,
            SourceId = dto.Source?.Id ?? 0,
            Release_Year = dto.ReleaseYear,
            Started_Airing = dto.StartedAiring,
            Finished_Airing = dto.FinishedAiring,
            Rating = dto.Rating,
            Studio = dto.Studio,
            Score = dto.Score,
            Status = dto.Status,
            Background = dto.Background,
            Trailer_url = dto.TrailerUrl,
            Trailer_embed_url = dto.TrailerEmbedUrl,
            Trailer_image_url = dto.TrailerImageUrl,

            Anime_Genres = dto.Genres?
                .Select(g => new AnimeGenre
                {
                    GenreId = g.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Genre = includeNavigation ? g.MapTo<Genre>() : null
                })
                .ToList() ?? new List<AnimeGenre>(),

            Anime_Producers = dto.Producers?
                .Select(p => new AnimeProducer
                {
                    ProducerId = p.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Producer = includeNavigation ? p.MapTo<Producer>() : null
                })
                .ToList() ?? new List<AnimeProducer>(),

            Anime_Licensors = dto.Licensors?
                .Select(l => new AnimeLicensor
                {
                    LicensorId = l.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Licensor = includeNavigation ? l.MapTo<Licensor>() : null
                })
                .ToList() ?? new List<AnimeLicensor>()
        };

        if (includeNavigation)
        {
            entity.Source = new Source { Id = dto.Source?.Id ?? 0, Name = dto.Source?.Name };
            entity.Type = new Type { Id = dto.Type?.Id ?? 0, Name = dto.Type?.Name };
        }
        
        return entity;
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
            Type = new TypeDto {Id = anime.Type?.Id ?? anime.TypeId, Name = anime.Type?.Name},
            Episodes = anime.Episodes,
            Duration = anime.Duration,
            Source = new SourceDto {Id = anime.Source?.Id ?? anime.SourceId, Name = anime.Source?.Name},
            ReleaseYear = anime.Release_Year,
            StartedAiring = anime.Started_Airing,
            FinishedAiring = anime.Finished_Airing,
            Rating = anime.Rating,
            Studio = anime.Studio,
            Score = anime.Score,
            Status = anime.Status,
            Background = anime.Background,
            TrailerUrl = anime.Trailer_url,
            TrailerEmbedUrl = anime.Trailer_embed_url,
            TrailerImageUrl = anime.Trailer_image_url,
            FavouritesCount = anime.Favourites.Count(),
            ReviewCount = anime.Favourites.Count(),
            
            Genres = anime.Anime_Genres
                .Select(ag => new GenreDto
                {
                    Id = ag.GenreId,
                    Name = ag?.Genre?.Name
                })
                .ToList(),
            
            Producers = anime.Anime_Producers
                .Select(ap => new ProducerDto
                {
                    Id = ap.ProducerId,
                    Name = ap?.Producer?.Name
                })
                .ToList(),
            
            Licensors = anime.Anime_Licensors
                .Select(al => new LicensorDto
                {
                    Id = al.LicensorId,
                    Name = al?.Licensor?.Name
                })
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