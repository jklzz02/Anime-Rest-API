using System.Linq.Expressions;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

public class AnimeHelper : IAnimeHelper
{
    private readonly IAnimeRepository _repository;
    private readonly IValidator<AnimeDto> _validator;
    private readonly IValidator<AnimeSearchParameters> _paramsValidator;
    
    public AnimeHelper(
        IAnimeRepository repository,
        IValidator<AnimeDto> validator,
        IValidator<AnimeSearchParameters> paramsValidator)
    {
        _repository = repository;
        _validator = validator;
        _paramsValidator = paramsValidator;
    }
    
    public async Task<AnimeDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }

    public async Task<PaginatedResult<AnimeDto>> GetAllAsync(int page, int size)
    {
        var result = await _repository
            .GetAllAsync(page, size);
        
        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<IEnumerable<AnimeDto>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var models = await _repository.GetByIdsAsync(ids);
        return models.ToDto();
    }

    public async Task<PaginatedResult<AnimeDto>> GetAllNonAdultAsync(int page, int size)
    {
        var result = await _repository
            .GetAllNonAdultAsync(page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByNameAsync(string name, int page, int size = 100)
    {
        var result = await _repository.GetByNameAsync(name, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByProducerAsync(int producerId, int page, int size = 100)
    {
        var result = await _repository.GetByProducerAsync(producerId, page, size);
        
        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByLicensorAsync(int licensorId, int page, int size = 100)
    {
        var result = await _repository.GetByLicensorAsync(licensorId, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByGenreAsync(int genreId, int page, int size = 100)
    {
        var result = await _repository.GetByGenreAsync(genreId, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetBySourceAsync(string source, int page, int size = 100)
    {
        var result = await _repository.GetBySourceAsync(source, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<PaginatedResult<AnimeDto>> GetByEnglishNameAsync(string englishName, int page, int size = 100)
    {
        var result = await _repository.GetByEnglishNameAsync(englishName, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByScoreAsync(int score, int page, int size = 100)
    {
        var result = await _repository.GetByScoreAsync(score, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByReleaseYearAsync(int year, int page, int size = 100)
    {
        var result = await _repository.GetByReleaseYearAsync(year, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    public async Task<PaginatedResult<AnimeDto>> GetByTypeAsync(string type, int page, int size = 100)
    {
        var result = await _repository.GetByTypeAsync(type, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }

    public async Task<PaginatedResult<AnimeDto>> GetByEpisodesAsync(int episodes, int page, int size = 100)
    {
        var result = await _repository.GetByEpisodesAsync(episodes, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }

        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
    
    
    public async Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var model = await _repository.GetFirstByConditionAsync(condition);
        return model?.ToDto();
    }

    public async Task<IEnumerable<AnimeDto>> GetMostRecentAsync(int count)
    {
        var models = await _repository.GetMostRecentAsync(count);
        return models.ToDto();
    }

    public async Task<IEnumerable<AnimeSummaryDto>> GetSummariesAsync(int count)
    {
        var models = await _repository
            .GetSummariesAsync(count);

        return models.ToDto();
    }

    public async Task<Result<AnimeDto>> CreateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
             List<Error> errors = validationResult.Errors
                 .ToJsonKeyedErrors<AnimeDto>()
                 .Select(pair => Error.Validation(pair.Key, pair.Value))
                 .ToList();
            
             return Result<AnimeDto>.Failure(errors);
        }
        
        var model = entity.ToModel(false);
        var result = await _repository.AddAsync(model);
        
        if (result.IsFailure)
        {
            return Result<AnimeDto>.Failure(result.Errors);
        }
        return Result<AnimeDto>.Success(result.Data.ToDto());
    }
    
    public async Task<Result<AnimeDto>> UpdateAsync(AnimeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<AnimeDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<AnimeDto>.Failure(errors);
        }
        
        var model = entity.ToModel();
        var result = await _repository.UpdateAsync(model);
        if (result.IsFailure)
        {
            return Result<AnimeDto>.Failure(result.Errors);
        }
        
        return Result<AnimeDto>.Success(result.Data.ToDto());
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<PaginatedResult<AnimeDto>> SearchAsync(
        AnimeSearchParameters parameters,
        int page,
        int size = 100)
    {
        var validationResult = await _paramsValidator.ValidateAsync(parameters);

        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<AnimeSearchParameters>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return new PaginatedResult<AnimeDto>(errors);
        }
        
        var result = await _repository.GetByParamsAsync(parameters, page, size);

        if (!result.Success)
        {
            return new PaginatedResult<AnimeDto>(result.Errors);
        }
        
        return new PaginatedResult<AnimeDto>(result.Items.ToDto(), page, size, result.TotalItems);
    }
}