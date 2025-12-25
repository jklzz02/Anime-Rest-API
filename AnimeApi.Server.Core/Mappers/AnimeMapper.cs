using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Core.Mappers;

/// <summary>
/// Provides mapping methods for converting between <see cref="Anime"/> and <see cref="AnimeDto"/> objects.
/// </summary>
public class AnimeMapper : Mapper<Anime, AnimeDto>, IAnimeMapper
{
    private readonly IMapper<Producer, ProducerDto> _producerMapper;
    private readonly IMapper<Licensor, LicensorDto> _licensorMapper;
    private readonly IMapper<Genre, GenreDto> _genreMapper;

    public AnimeMapper(
        IMapper<Producer, ProducerDto> producerMapper,
        IMapper<Licensor, LicensorDto> licensorMapper,
        IMapper<Genre, GenreDto> genreMapper)
    {
        _producerMapper = producerMapper;
        _licensorMapper = licensorMapper;
        _genreMapper = genreMapper;
    }

    public Anime MapToEntity(AnimeDto dto, bool includeNavigation)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            Started_Airing = dto.StartedAiring?.ToUniversalTime(),
            Finished_Airing = dto.FinishedAiring?.ToUniversalTime(),
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
                    Genre = includeNavigation? _genreMapper.MapToEntity(g) : null
                })
                .ToList() ?? new List<AnimeGenre>(),

            Anime_Producers = dto.Producers?
                .Select(p => new AnimeProducer
                {
                    ProducerId = p.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Producer = includeNavigation ? _producerMapper.MapToEntity(p) : null
                })
                .ToList() ?? new List<AnimeProducer>(),

            Anime_Licensors = dto.Licensors?
                .Select(l => new AnimeLicensor
                {
                    LicensorId = l.Id ?? 0,
                    AnimeId = dto.Id ?? 0,
                    Licensor = includeNavigation ? _licensorMapper.MapToEntity(l) : null
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

    public override Anime MapToEntity(AnimeDto entity)
        => MapToEntity(entity, true);

    public IEnumerable<Anime> MapToEntity(IEnumerable<AnimeDto> anime, bool includeNavigation)
        => anime.Select(a => MapToEntity(a, includeNavigation));
}