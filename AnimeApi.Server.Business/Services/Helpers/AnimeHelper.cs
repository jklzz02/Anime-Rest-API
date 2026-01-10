using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Objects.Partials;
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
        var query = new AnimeQuery()
            .ByPk(id)
            .IncludeFullRelation();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids)
        =>  GetByIdAsync(
            ids,
            Constants.OrderBy.Fields.Score,
            Constants.OrderBy.StringDirections.Descending);

    public async Task<IEnumerable<AnimeDto>> GetByIdAsync(IEnumerable<int> ids, string orderBy, string direction)
    {
        var  query = new AnimeQuery()
            .ByPk(ids)
            .IncludeFullRelation()
            .Sorting(orderBy, direction);

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<AnimeDto>> GetAllAsync()
    {
        return await 
            _repository.GetAllAsync();
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size)
        => await GetAllAsync(page, size, false);

    public async Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size, bool includeAdult)
    {
        var count = await
            _repository
                .CountAsync(new AnimeQuery()
                    .ExcludeAdultContent(!includeAdult));
        
        var query = new AnimeQuery()
            .AsSplitQuery()
            .ExcludeAdultContent(!includeAdult)
            .Popular()
            .TieBreaker()
            .Paginate(page, size)
            .IncludeFullRelation();

        var items = await
            _repository.FindAsync(query);

        return new PaginatedResult<AnimeDto>(items, page, size, count);
    }

    public async Task<IEnumerable<AnimeListItem>> GetAnimeListAsync(int count)
    {
        var query = new AnimeQuery()
            .Popular()
            .Recent()
            .TieBreaker()
            .Limit(count);
        
        return await
            _repository.FindAsync<AnimeListItem>(query);
    }

    public async Task<IEnumerable<AnimeListItem>> GetAnimeListByQueryAsync(string textQuery, int count)
    {
        var query = new AnimeQuery()
            .FullTextSearch(textQuery)
            .Popular()
            .Recent()
            .TieBreaker()
            .Limit(count);
        
        return await
            _repository.FindAsync<AnimeListItem>(query);
    }

    public async Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count)
    {
        var query = new AnimeQuery()
            .Recents(count)
            .IncludeFullRelation();

        return await
            _repository.FindAsync(query);
    }

    public async Task<AnimeSummary?> GetSummaryByIdAsync(int id)
    {
        return await
            _repository.FindFirstOrDefaultAsync<AnimeSummary>(new AnimeQuery().ByPk(id));
    }
    
    public async Task<IEnumerable<AnimeSummary>> GetSummariesByIdAsync(IEnumerable<int> ids)
        => await GetSummariesByIdAsync(
            ids,
            Constants.OrderBy.Fields.Score,
            Constants.OrderBy.StringDirections.Descending);

    public async Task<IEnumerable<AnimeSummary>> GetSummariesByIdAsync(
        IEnumerable<int> ids,
        string orderBy,
        string direction)
    {
        var query = new AnimeQuery()
            .ByPk(ids)
            .Sorting(orderBy, direction);
        
        return await
            _repository.FindAsync<AnimeSummary>(query);
    }

    public async Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count)
    {
        var query = new AnimeQuery()
            .IncludeFullRelation()
            .Popular()
            .TieBreaker()
            .Limit(count);

        return await
            _repository.FindAsync<AnimeSummary>(query);
    }

    public async Task<PaginatedResult<AnimeSummary>> GetSummariesAsync(int page, int size)
    {
        var count = await
            _repository.CountAsync();
        
        var query = new AnimeQuery()
            .IncludeFullRelation()
            .Popular()
            .TieBreaker()
            .Paginate(page, size);
        
        var items = await
            _repository.FindAsync<AnimeSummary>(query);
        
        return new PaginatedResult<AnimeSummary>(items, page, size, count);
    }

    public async Task<Result<AnimeDto>> CreateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
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
        ArgumentNullException.ThrowIfNull(entity);
        
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
            _repository.DeleteAsync(new AnimeQuery().ByPk(id));
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
            .FullTextSearch(parameters.Query)
            .Name(parameters.Name)
            .EnglishName(parameters.EnglishName)
            .Source(parameters.Source)
            .Type(parameters.Type)
            .Status(parameters.Status)
            .Studio(parameters.Studio)
            .ScoreRange(parameters.MinScore, parameters.MaxScore)
            .EpisodeRange(parameters.MinEpisodes, parameters.MaxEpisodes, parameters.Episodes)
            .YearRange(parameters.MinReleaseYear, parameters.MaxReleaseYear)
            .AirDateRange(
                parameters.StartDateFrom,
                parameters.StartDateTo,
                parameters.EndDateFrom,
                parameters.EndDateTo)
            .Genres(
                parameters.GenreId,
                parameters.GenreName,
                parameters.GenreNames)
            .Producers(
                parameters.ProducerId,
                parameters.ProducerName,
                parameters.ProducerNames)
            .Licensors(
                parameters.LicensorId,
                parameters.LicensorName,
                parameters.LicensorNames)
            .Sorting(
                parameters.OrderBy,
                parameters.SortOrder)
            .ExcludeAdultContent(!parameters.IncludeAdultContent);

        var count = await
            _repository.CountAsync(query);

        var items = await
            _repository.FindAsync(
                query
                    .TieBreaker()
                    .Paginate(page, size)
                    .IncludeFullRelation());

        return new PaginatedResult<AnimeDto>(items, page, size, count);
    }
}