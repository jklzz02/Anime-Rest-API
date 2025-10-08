using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class ProducerHelper : IProducerHelper
{
    private readonly IProducerRepository _repository;
    private readonly IBaseValidator<ProducerDto> _validator;
    public ProducerHelper(IProducerRepository repository, IBaseValidator<ProducerDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.MapTo<ProducerDto>();
    }

    public async Task<IEnumerable<ProducerDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.MapTo<ProducerDto>();
    }

    public async Task<IEnumerable<ProducerDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.MapTo<ProducerDto>();
    }

    public async Task<Result<ProducerDto>> CreateAsync(ProducerDto entity)
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
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var model = entity.MapTo<Producer>();
        var result = await _repository.AddAsync(model);

        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<ProducerDto>();
        return Result<ProducerDto>.Success(data);
    }

    public async Task<Result<ProducerDto>> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<ProducerDto>.Failure(errors);
        };

        
        var model = entity.MapTo<Producer>();
        var result = await _repository.UpdateAsync(model);
        
        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<ProducerDto>();
        return Result<ProducerDto>.Success(data);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}