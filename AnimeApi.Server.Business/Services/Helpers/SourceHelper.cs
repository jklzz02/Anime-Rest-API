using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class SourceHelper : ISourceHelper
{
    private readonly ISourceRepository _repository;
    private readonly IBaseValidator<SourceDto> _validator;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    
    public SourceHelper(ISourceRepository repository, IBaseValidator<SourceDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<SourceDto?> GetByIdAsync(int id)
    {
        var model =  await _repository.GetByIdAsync(id);
        return model?.MapTo<SourceDto>();
    }

    public async Task<IEnumerable<SourceDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.MapTo<SourceDto>();
    }

    public async Task<IEnumerable<SourceDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.MapTo<SourceDto>();
    }

    public async Task<SourceDto?> CreateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var ids = await _repository.GetExistingIdsAsync();
        var names = await _repository.GetExistingNamesAsync();

        _validator
            .WithExistingIds(ids)
            .WithExistingNames(names);
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
           ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<SourceDto>();
           return null;
        }
        
        var model = entity.MapTo<Source>();
        var result = await _repository.AddAsync(model);

        if (result == null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return result.MapTo<SourceDto>();
    }

    public async Task<SourceDto?> UpdateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors.ToJsonKeyedErrors<SourceDto>();
            return null;
        }

        var model = entity.MapTo<Source>();
        var result = await _repository.UpdateAsync(model);
        
        if(result is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return result.MapTo<SourceDto>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}