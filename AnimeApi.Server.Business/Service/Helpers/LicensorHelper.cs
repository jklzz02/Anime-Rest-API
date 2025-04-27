using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions.Mapper;
using AnimeApi.Server.Business.Service.Interfaces;
using AnimeApi.Server.Business.Validator.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Service.Helpers;

public class LicensorHelper : ILicensorHelper
{
    private readonly ILicensorRepository _repository;
    private readonly ILicensorValidator _validator;
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

    public async Task<bool> CreateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if(!validationResult.IsValid) return false;
        
        var model = entity.ToModel();
        return await _repository.AddAsync(model);
    }

    public async Task<bool> UpdateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if(!validationResult.IsValid) return false;
        
        var model = entity.ToModel();
        return await _repository.UpdateAsync(model);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}