using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services.Helpers;

public class GenreHelper : IGenreHelper
{
    private readonly IRepository<Genre, GenreDto> _repository;
    private readonly IBaseValidator<GenreDto> _validator;
    public GenreHelper(
        IRepository<Genre, GenreDto> repository,
        IBaseValidator<GenreDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .ToQuerySpec<Genre>();

        return await 
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<GenreDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        var query = new BaseQuery()
            .ByName(name)
            .ToQuerySpec<Genre>();

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        return await 
            _repository.GetAllAsync();
    }

    public async Task<Result<GenreDto>> CreateAsync(GenreDto entity)
    {
      
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var existing = await 
            _repository.GetAllAsync();

        _validator
            .WithExistingIds(existing.Select(g => g.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(g => g.Name));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<GenreDto>();
            
            return Result<GenreDto>.Failure(errors);
        }

        var result = await _repository.AddAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<GenreDto>.Failure(result.Errors);
        }
        
        return result;
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

        var result = await _repository.UpdateAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<GenreDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .ToQuerySpec<Genre>();

        return await _repository.DeleteAsync(query);
    }
}