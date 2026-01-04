using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.SpecHelpers;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

/// <summary>
/// Represents the repository implementation for managing <see cref="Anime"/> entities within the data access layer.
/// </summary>
/// <remarks>
/// This class extends the generic <see cref="Repository{TEntity, TDto}"/> to provide specific functionality
/// for handling <see cref="Anime"/> entities and their corresponding <see cref="AnimeDto"/> models.
/// </remarks>
public class AnimeRepository : Repository<Anime, AnimeDto>
{
    /// <summary>
    /// Provides a repository for managing <see cref="Anime"/> entities and their corresponding data transfer objects (<see cref="AnimeDto"/>).
    /// </summary>
    /// <remarks>
    /// The <see cref="AnimeRepository"/> class includes specific operations and behavior tailored to the <see cref="Anime"/> entity
    /// and its associated business logic, utilizing the provided <see cref="IAnimeMapper"/> for mapping.
    /// </remarks>
    public AnimeRepository(AnimeDbContext context, IAnimeMapper mapper)
        : base(context, mapper)
    {
    }

    /// <inheritdoc />
    public override  async Task<Result<AnimeDto>> AddAsync(AnimeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = Mapper
            .AsSpecific<IAnimeMapper>()
            .MapToEntity(dto, false);
        
        var anime = await
            Context.Anime.FirstOrDefaultAsync(a => a.Id == entity.Id);

        if (anime != null)
        {
            return Result<AnimeDto>.ValidationFailure(
                "Anime already exists", 
                $"There is already an anime with the specified id '{dto.Id}'");
        }
        
        var createdEntry = await
            Context.AddAsync(entity);
        
       bool result = await 
           Context.SaveChangesAsync() > 0;

       if (!result)
       {
          return Result<AnimeDto>.InternalFailure("Create", "No entity created");
       }

       var resultDto = Mapper.MapToDto(createdEntry.Entity);
       
       Context.ChangeTracker.Clear();
       return Result<AnimeDto>.Success(resultDto);
    }

    /// <inheritdoc />
    public override async Task<Result<AnimeDto>> UpdateAsync(AnimeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.Id.GetValueOrDefault() == 0)
        {
            return Result<AnimeDto>.ValidationFailure(
                "Anime must have an ID", 
                "Cannot update unexisting entry");
        }

        var  entity = Mapper
            .AsSpecific<IAnimeMapper>()
            .MapToEntity(dto, false);
        
        var anime = await
            new AnimeQuery()
                .ByPk(dto.Id.GetValueOrDefault())
                .IncludeFullRelation()
                .Tracked()
                .Apply(Context.Anime)
                .FirstOrDefaultAsync();
        
        if (anime is null)
        {
            return Result<AnimeDto>.InternalFailure("update", $"there's no anime with id '{entity.Id}'.");
        }

        UpdateAnime(anime, entity);
        await UpdateRelations(anime.AnimeGenres.ToList(), entity.AnimeGenres.ToList());
        await UpdateRelations(anime.AnimeProducers.ToList(), entity.AnimeProducers.ToList());
        await UpdateRelations(anime.AnimeLicensors.ToList(), entity.AnimeLicensors.ToList());

        var result = await Context.SaveChangesAsync() > 0;

        if (!result)
        {
            return Result<AnimeDto>.InternalFailure("update", "something went wrong during entity update.");
        }
        
        var resultDto = Mapper.MapToDto(anime);
        
        Context.ChangeTracker.Clear();
        return Result<AnimeDto>.Success(resultDto);
    }

    private void UpdateAnime(Anime original, Anime updated)
    {
        original.Name = updated.Name;
        original.EnglishName = updated.EnglishName;
        original.OtherName = updated.OtherName;
        original.Synopsis = updated.Synopsis;
        original.ImageUrl = updated.ImageUrl;
        original.TypeId = updated.TypeId;
        original.SourceId = updated.SourceId;
        original.Episodes = updated.Episodes;
        original.Duration = updated.Duration;
        original.SourceId = updated.SourceId;
        original.ReleaseYear = updated.ReleaseYear;
        original.StartedAiring = updated.StartedAiring;
        original.FinishedAiring = updated.FinishedAiring;
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