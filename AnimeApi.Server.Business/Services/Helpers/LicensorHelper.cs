using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Business.Services.Helpers;

public class LicensorHelper : ILicensorHelper
{
    private readonly ILicensorRepository _repository;
    private readonly IBaseValidator<LicensorDto> _validator;
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

    public async Task<Result<LicensorDto>> CreateAsync(LicensorDto entity)
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
                .ToJsonKeyedErrors<LicensorDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();

            return Result<LicensorDto>.Failure(errors);
        }
        
        var model = entity.MapTo<Licensor>();
        var result = await _repository.AddAsync(model);

        if (result.IsFailure)
        {
            return Result<LicensorDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<LicensorDto>();
        return Result<LicensorDto>.Success(data);
    }

    public async Task<Result<LicensorDto>> UpdateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<LicensorDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<LicensorDto>.Failure(errors);
        }
        
        var model = entity.MapTo<Licensor>();
        var result = await _repository.UpdateAsync(model);

        if (result.IsFailure)
        {
            return Result<LicensorDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<LicensorDto>();
        return Result<LicensorDto>.Success(data);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}