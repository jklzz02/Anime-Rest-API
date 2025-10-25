using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

public class AnimeHelper : IAnimeHelper
{
    private readonly IRepository<Anime, AnimeDto> _repository;
    private readonly IValidator<AnimeDto> _validator;
    private readonly IValidator<AnimeSearchParameters> _paramsValidator;
    
    public AnimeHelper(
        IRepository<Anime, AnimeDto> repository,
        IValidator<AnimeDto> validator,
        IValidator<AnimeSearchParameters> paramsValidator)
    {
        _repository = repository;
        _validator = validator;
        _paramsValidator = paramsValidator;
    }
    
    public async Task<AnimeDto?> GetByIdAsync(int id)
    {
        var query = AnimeQuery.ByPk(id)
            .IncludeFullRelation();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<AnimeDto>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var query = AnimeQuery.ByPk(ids)
            .IncludeFullRelation();

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<AnimeDto>> GetAllAsync()
    {
        return await 
            _repository.GetAllAsync();
    }

    public async Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size)
    {
        var count = await
            _repository.CountAsync();

        var query = new AnimeQuery()
            .AsSplitQuery()
            .SortBy(a => a.Score, SortDirections.Desc)
            .Paginate(page, size)
            .IncludeFullRelation();

        var items = await
            _repository.FindAsync(query);

        return new PaginatedResult<AnimeDto>(items, page, size, count);
    }

    public async Task<PaginatedResult<AnimeDto>> GetAllNonAdultAsync(int page, int size)
    {
        var query = new AnimeQuery()
            .AsSplitQuery()
            .ExcludeAdultContent();

        var count = await
            _repository.CountAsync(query);

        var items = await
            _repository.FindAsync(
                query
                    .SortBy(a => a.Score, SortDirections.Desc)
                    .Paginate(page, size)
                    .IncludeFullRelation());


        return new PaginatedResult<AnimeDto>(items, page, size, count);
    }

    public async Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count)
    {
        var query = new AnimeQuery()
            .Recents(count)
            .IncludeFullRelation();

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count)
    {
        var query = new AnimeQuery()
            .IncludeFullRelation()
            .SortBy(a => a.Score, SortDirections.Desc)
            .Limit(count);

        var result = await
            _repository.FindAsync(query);

        return result.ToSummary();
    }

    public async Task<Result<AnimeDto>> CreateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<AnimeDto>();
            
             return Result<AnimeDto>.Failure(errors);
        }

        var result = await _repository.AddAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<AnimeDto>.Failure(result.Errors);
        }

        return result;
    }
    
    public async Task<Result<AnimeDto>> UpdateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<AnimeDto>();
            
            return Result<AnimeDto>.Failure(errors);
        }
        
        var result = await _repository.UpdateAsync(entity);
        if (result.IsFailure)
        {
            return Result<AnimeDto>.Failure(result.Errors);
        }
        
        return result;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await 
            _repository.DeleteAsync(AnimeQuery.ByPk(id));
    }

    public async Task<PaginatedResult<AnimeDto>> SearchAsync(
        AnimeSearchParameters parameters,
        int page,
        int size = 100)
    {
        var validationResult = await 
            _paramsValidator.ValidateAsync(parameters);

        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<AnimeSearchParameters>();
            
            return new PaginatedResult<AnimeDto>(errors);
        }

        var query = new AnimeQuery()
            .AsSplitQuery()
            .WithFullTextSearch(parameters.Query)
            .WithName(parameters.Name)
            .WithEnglishName(parameters.EnglishName)
            .WithSource(parameters.Source)
            .WithType(parameters.Type)
            .WithStatus(parameters.Status)
            .WithStudio(parameters.Studio)
            .WithScoreRange(parameters.MinScore, parameters.MaxScore)
            .WithEpisodeRange(parameters.MinEpisodes, parameters.MaxEpisodes, parameters.Episodes)
            .WithYearRange(parameters.MinReleaseYear, parameters.MaxReleaseYear)
            .WithAirDateRange(
                parameters.StartDateFrom,
                parameters.StartDateTo,
                parameters.EndDateFrom,
                parameters.EndDateTo)
            .WithGenres(
                parameters.GenreId,
                parameters.GenreName,
                parameters.GenreNames)
            .WithProducers(
                parameters.ProducerId,
                parameters.ProducerName,
                parameters.ProducerNames)
            .WithLicensors(
                parameters.LicensorId,
                parameters.LicensorName,
                parameters.LicensorNames)
            .ExcludeAdultContent(!parameters.IncludeAdultContext);

        var count = await
            _repository.CountAsync(query);

        var items = await
            _repository.FindAsync(
                query
                    .Paginate(page, size)
                    .IncludeFullRelation());

        return new PaginatedResult<AnimeDto>(items, page, size, count);
    }
}