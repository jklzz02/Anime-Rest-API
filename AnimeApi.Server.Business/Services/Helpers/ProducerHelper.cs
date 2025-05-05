using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

public class ProducerHelper : IProducerHelper
{
    private readonly IProducerRepository _repository;
    private readonly IProducerValidator _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    public ProducerHelper(IProducerRepository repository, IProducerValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.ToDto();
    }

    public async Task<IEnumerable<ProducerDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.ToDto();
    }

    public async Task<IEnumerable<ProducerDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.ToDto();
    }

    public async Task<bool> CreateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<ProducerDto>();
            return false;
        };
        
        var model = entity.ToModel();
        return await _repository.AddAsync(model);
    }

    public async Task<bool> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<ProducerDto>();
            return false;
        }
        
        var model = entity.ToModel();
        return await _repository.UpdateAsync(model);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}