using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Business.Services.Helpers;

public class TypeHelper : ITypeHelper
{
    private readonly ITypeRepository _repository;
    private readonly IBaseValidator<TypeDto> _validator;

    public TypeHelper(ITypeRepository repository, IBaseValidator<TypeDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<TypeDto?> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);
        return model?.MapTo<TypeDto>();
    }

    public async Task<IEnumerable<TypeDto>> GetByNameAsync(string name)
    {
        var models = await _repository.GetByNameAsync(name);
        return models.MapTo<TypeDto>();
    }

    public async Task<IEnumerable<TypeDto>> GetAllAsync()
    {
        var models = await _repository.GetAllAsync();
        return models.MapTo<TypeDto>();
    }

    public async Task<Result<TypeDto>> CreateAsync(TypeDto entity)
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
                .ToJsonKeyedErrors<TypeDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<TypeDto>.Failure(errors);
        }
        
        var model = entity.MapTo<Type>();
        var result = await _repository.AddAsync(model);

        if (result.IsFailure)
        {
            return Result<TypeDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<TypeDto>();
        return Result<TypeDto>.Success(data);
    }

    public async Task<Result<TypeDto>> UpdateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<TypeDto>()
                .Select(pair => Error.Validation(pair.Key, pair.Value))
                .ToList();
            
            return Result<TypeDto>.Failure(errors);
        }

        var model = entity.MapTo<Type>();
        var result = await _repository.UpdateAsync(model);
        
        if (result.IsFailure)
        {
            return Result<TypeDto>.Failure(result.Errors);
        }
        
        var data = result.Data.MapTo<TypeDto>();
        return Result<TypeDto>.Success(data);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}