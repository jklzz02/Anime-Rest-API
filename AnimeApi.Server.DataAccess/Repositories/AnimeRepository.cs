using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.DataAccess.Repositories;

/// <summary>
/// Represents a repository for managing and querying Anime data.
/// </summary>
/// <remarks>
/// This class provides the implementation of the <see cref="Repository{TEntity, TDto}"/>
/// and serves as a mediator between the database and application logic,
/// enabling operations such as retrieval, addition, update, and deletion of <see cref="Anime"/> entities.
/// </remarks>
public class AnimeRepository : Repository<Anime, AnimeDto>
{
    private readonly IAnimeMapper _animeMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for anime data operations.</param>
    public AnimeRepository(AnimeDbContext context, IAnimeMapper mapper)
        : base(context, mapper)
    {
        _animeMapper = mapper;
    }

    /// <inheritdoc />
    public override async Task<Result<AnimeDto>> UpdateAsync(AnimeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var entity = _animeMapper.MapToEntity(dto);

        var anime = await 
            FindFirstOrDefaultAsync(AnimeQuery.ByPk(entity.Id).Tracked());
        
        if (anime is null)
        {
            return Result<AnimeDto>.InternalFailure("update", $"there's no anime with id '{entity.Id}'.");
        }

        var existingEntity = _animeMapper.MapToEntity(anime, false);


        UpdateAnime(existingEntity, entity);
        await UpdateRelations(existingEntity.Anime_Genres.ToList(), entity.Anime_Genres.ToList());
        await UpdateRelations(existingEntity.Anime_Producers.ToList(), entity.Anime_Producers.ToList());
        await UpdateRelations(existingEntity.Anime_Licensors.ToList(), entity.Anime_Licensors.ToList());

        var result = await Context.SaveChangesAsync() > 0;

        if (!result)
        {
            return Result<AnimeDto>.InternalFailure("update", "something went wrong during entity update.");
        }
        
        Context.ChangeTracker.Clear();
        return Result<AnimeDto>.Success(anime);
    }

    private void UpdateAnime(Anime original, Anime updated)
    {
        original.Name = updated.Name;
        original.English_Name = updated.English_Name;
        original.Other_Name = updated.Other_Name;
        original.Synopsis = updated.Synopsis;
        original.Image_URL = updated.Image_URL;
        original.TypeId = updated.TypeId;
        original.SourceId = updated.SourceId;
        original.Episodes = updated.Episodes;
        original.Duration = updated.Duration;
        original.SourceId = updated.SourceId;
        original.Release_Year = updated.Release_Year;
        original.Started_Airing = updated.Started_Airing;
        original.Finished_Airing = updated.Finished_Airing;
        original.Rating = updated.Rating;
        original.Studio = updated.Studio;
        original.Score = updated.Score;
        original.Status = updated.Status;
    }

   private async Task UpdateRelations<T>(
    List<T> original,
    List<T> updated)
    where T : class, IAnimeRelation, new()
    {
        var originalIds = original.Select(a => a.RelatedId).ToList();
        var updatedIds = updated.Select(a => a.RelatedId).ToList();
        
        if (originalIds.SequenceEqual(updatedIds))
        {
            return;
        }
        
        var animeId = original.FirstOrDefault()?.AnimeId ?? updated.FirstOrDefault()?.AnimeId ?? 0;
        
        var toRemove = original.Where(o => !updatedIds.Contains(o.RelatedId)).ToList();
        
        toRemove.ForEach(x => Context.Set<T>().Remove(x));
        
        var idsToAdd = updatedIds.Where(id => !originalIds.Contains(id)).ToList();

        var newRelations = idsToAdd
            .Select(id => new T { AnimeId = animeId, RelatedId = id });
            
        await Context.Set<T>().AddRangeAsync(newRelations);
    }
}