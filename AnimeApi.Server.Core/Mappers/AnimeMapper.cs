using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Core.Mappers;

/// <summary>
/// Provides mapping methods for converting between <see cref="Anime"/> and <see cref="AnimeDto"/> objects.
/// </summary>
public class AnimeMapper(
    IMapper<Producer, ProducerDto> producerMapper,
    IMapper<Licensor, LicensorDto> licensorMapper,
    IMapper<Genre, GenreDto> genreMapper)
    : Mapper<Anime, AnimeDto>, IAnimeMapper
{
    public Anime MapToEntity(AnimeDto dto, bool includeNavigation)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity =  new Anime
        {
            Id = dto.Id ?? 0,
            Name = dto.Name,
            EnglishName = dto.EnglishName,
            OtherName = dto.OtherName,
            Synopsis = dto.Synopsis,
            ImageUrl = dto.ImageUrl,
            TypeId = dto.Type?.Id ?? 0,
            Episodes = dto.Episodes,
            Duration = dto.Duration,
            SourceId = dto.Source?.Id ?? 0,
            ReleaseYear = dto.ReleaseYear,
            StartedAiring = dto.StartedAiring?.ToUniversalTime(),
            FinishedAiring = dto.FinishedAiring?.ToUniversalTime(),
            Rating = dto.Rating,
            Studio = dto.Studio,
            Score = dto.Score,
            Status = dto.Status,
            Background = dto.Background,
            TrailerUrl = dto.TrailerUrl,
            TrailerEmbedUrl = dto.TrailerEmbedUrl,
            TrailerImageUrl = dto.TrailerImageUrl,

            AnimeGenres = dto.Genres?
                .Select(g => new AnimeGenre
                {
                    GenreId = g.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Genre = includeNavigation? genreMapper.MapToEntity(g) : null
                })
                .ToList() ?? new List<AnimeGenre>(),

            AnimeProducers = dto.Producers?
                .Select(p => new AnimeProducer
                {
                    ProducerId = p.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Producer = includeNavigation ? producerMapper.MapToEntity(p) : null
                })
                .ToList() ?? new List<AnimeProducer>(),

            AnimeLicensors = dto.Licensors?
                .Select(l => new AnimeLicensor
                {
                    LicensorId = l.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Licensor = includeNavigation ? licensorMapper.MapToEntity(l) : null
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
    
    public override AnimeDto MapToDto(Anime anime)
    {
        ArgumentNullException.ThrowIfNull(anime);

        return new AnimeDto()
        {
            Id = anime.Id,
            Name = anime.Name,
            EnglishName = anime.EnglishName,
            OtherName = anime.OtherName,
            Synopsis = anime.Synopsis,
            ImageUrl = anime.ImageUrl,
            Type = new TypeDto {Id = anime.Type?.Id ?? anime.TypeId, Name = anime.Type?.Name},
            Episodes = anime.Episodes,
            Duration = anime.Duration,
            Source = new SourceDto {Id = anime.Source?.Id ?? anime.SourceId, Name = anime.Source?.Name},
            ReleaseYear = anime.ReleaseYear,
            StartedAiring = anime.StartedAiring,
            FinishedAiring = anime.FinishedAiring,
            Rating = anime.Rating,
            Studio = anime.Studio,
            Score = anime.Score,
            Status = anime.Status,
            Background = anime.Background,
            TrailerUrl = anime.TrailerUrl,
            TrailerEmbedUrl = anime.TrailerEmbedUrl,
            TrailerImageUrl = anime.TrailerImageUrl,
            FavouritesCount = anime.Favourites.Count(),
            ReviewCount = anime.Favourites.Count(),
            
            Genres = anime.AnimeGenres
                .Select(ag => new GenreDto
                {
                    Id = ag.GenreId,
                    Name = ag?.Genre?.Name
                })
                .ToList(),
            
            Producers = anime.AnimeProducers
                .Select(ap => new ProducerDto
                {
                    Id = ap.ProducerId,
                    Name = ap?.Producer?.Name
                })
                .ToList(),
            
            Licensors = anime.AnimeLicensors
                .Select(al => new LicensorDto
                {
                    Id = al.LicensorId,
                    Name = al?.Licensor?.Name
                })
                .ToList()
        };
    }

    public override Anime MapToEntity(AnimeDto entity)
        => MapToEntity(entity, true);

    public IEnumerable<Anime> MapToEntity(IEnumerable<AnimeDto> anime, bool includeNavigation)
        => anime.Select(a => MapToEntity(a, includeNavigation));
}