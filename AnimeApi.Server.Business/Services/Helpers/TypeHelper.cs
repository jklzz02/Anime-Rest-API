using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

public class TypeHelper : ITypeHelper
{
    private readonly ITypeRepository _repository;
    private readonly IBaseValidator<TypeDto> _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();

    public TypeHelper(ITypeRepository repository, IBaseValidator<TypeDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<TypeDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<TypeDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.ToDto();
    }

    public async Task<IEnumerable<TypeDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }

    public async Task<TypeDto?> CreateAsync(TypeDto entity)
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
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<TypeDto>();
            return null;
        }
        
        var model = entity.ToModel();
        var result = await _repository.AddAsync(model);

        if (result == null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return result.ToDto();
    }

    public async Task<TypeDto?> UpdateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<TypeDto>();
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