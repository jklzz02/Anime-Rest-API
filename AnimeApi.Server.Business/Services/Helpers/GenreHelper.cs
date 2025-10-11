using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class GenreHelper : IGenreHelper
{
    private readonly IGenreRepository _repository;
    private readonly IBaseValidator<GenreDto> _validator;
    public GenreHelper(IGenreRepository repository, IBaseValidator<GenreDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.MapTo<GenreDto>();
    }

    public async Task<IEnumerable<GenreDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        var models = await _repository.GetByNameAsync(name);
        return models.MapTo<GenreDto>();
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.MapTo<GenreDto>();
    }

    public async Task<Result<GenreDto>> CreateAsync(GenreDto entity)
    {
      
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var ids = await _repository.GetExistingIdsAsync();
        var names = await _repository.GetExistingNamesAsync();

        _validator
            .WithExistingIds(ids)
            .WithExistingNames(names);
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<GenreDto>();
            
            return Result<GenreDto>.Failure(errors);
        }

        var model = entity.MapTo<Genre>();
        var result = await _repository.AddAsync(model);
        
        if (result.IsFailure)
        {
            return Result<GenreDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<GenreDto>();
        return Result<GenreDto>.Success(data);
    }

    public async Task<Result<GenreDto>> UpdateAsync(GenreDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<GenreDto>();
            
            return Result<GenreDto>.Failure(errors);
        }

        var model = entity.MapTo<Genre>();
        var result = await _repository.UpdateAsync(model);
        
        if (result.IsFailure)
        {
            return Result<GenreDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<GenreDto>();
        return Result<GenreDto>.Success(data);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}