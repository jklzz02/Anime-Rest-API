using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Objects;

public class GenreHelper : IGenreHelper
{
    private readonly IGenreRepository _repository;
    private readonly IBaseValidator<GenreDto> _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    public GenreHelper(IGenreRepository repository, IBaseValidator<GenreDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<GenreDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        var models = await _repository.GetByNameAsync(name);
        return models.ToDto();
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }

    public async Task<GenreDto?> CreateAsync(GenreDto entity)
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
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<GenreDto>();
            return null;
        }

        var model = entity.ToModel();
        var result = await _repository.AddAsync(model);
        
        if(result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;    
        }
        
        return result.ToDto();
    }

    public async Task<GenreDto?> UpdateAsync(GenreDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<GenreDto>();
            return null;
        }

        var model = entity.ToModel();
        var result = await _repository.UpdateAsync(model);
        
        if(result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return result.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}