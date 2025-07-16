using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Helpers;

public class ProducerHelper : IProducerHelper
{
    private readonly IProducerRepository _repository;
    private readonly IBaseValidator<ProducerDto> _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    public ProducerHelper(IProducerRepository repository, IBaseValidator<ProducerDto> validator)
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

    public async Task<ProducerDto?> CreateAsync(ProducerDto entity)
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
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<ProducerDto>();
            return null;
        };
        
        var model = entity.ToModel();
        var result = await _repository.AddAsync(model);

        if (result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return model.ToDto();
    }

    public async Task<ProducerDto?> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<ProducerDto>();
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