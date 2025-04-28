using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

public class GenreHelper : IGenreHelper
{
    private readonly IGenreRepository _repository;
    private readonly IGenreValidator _validator;

    public GenreHelper(IGenreRepository repository, IGenreValidator validator)
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

    public async Task<bool> CreateAsync(GenreDto entity)
    {
      
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if(!validationResult.IsValid) return false;

        var model = entity.ToModel();
        return await _repository.AddAsync(model);
    }

    public async Task<bool> UpdateAsync(GenreDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid) return false;

        var model = entity.ToModel();
        return await _repository.UpdateAsync(model);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}