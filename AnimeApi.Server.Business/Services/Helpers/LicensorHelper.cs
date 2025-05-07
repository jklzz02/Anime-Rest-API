using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

public class LicensorHelper : ILicensorHelper
{
    private readonly ILicensorRepository _repository;
    private readonly ILicensorValidator _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    public LicensorHelper(ILicensorRepository repository, ILicensorValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<LicensorDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<LicensorDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.ToDto();
    }
    
    public async Task<IEnumerable<LicensorDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }

    public async Task<LicensorDto?> CreateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<LicensorDto>();
            return null;
        }
        
        var model = entity.ToModel();
        var result = await _repository.AddAsync(model);

        if (result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.ToDto();
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
        
        var model = entity.ToModel();
        var result = await _repository.UpdateAsync(model);

        if (result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}