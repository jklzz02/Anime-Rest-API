using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class LicensorHelper : ILicensorHelper
{
    private readonly ILicensorRepository _repository;
    private readonly IBaseValidator<LicensorDto> _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    public LicensorHelper(ILicensorRepository repository, IBaseValidator<LicensorDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<LicensorDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.MapTo<LicensorDto>();
    }

    public async Task<IEnumerable<LicensorDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.MapTo<LicensorDto>();
    }
    
    public async Task<IEnumerable<LicensorDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.MapTo<LicensorDto>();
    }

    public async Task<LicensorDto?> CreateAsync(LicensorDto entity)
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
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<LicensorDto>();
            return null;
        }
        
        var model = entity.MapTo<Licensor>();
        var result = await _repository.AddAsync(model);

        if (result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.MapTo<LicensorDto>();
    }

    public async Task<LicensorDto?> UpdateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<LicensorDto>();
            return null;
        }
        
        var model = entity.MapTo<Licensor>();
        var result = await _repository.UpdateAsync(model);

        if (result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.MapTo<LicensorDto>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}