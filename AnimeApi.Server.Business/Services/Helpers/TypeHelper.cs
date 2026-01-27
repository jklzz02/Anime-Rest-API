using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.SpecHelpers;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Business.Services.Helpers;

public class TypeHelper(
    IRepository<Type, TypeDto> repository,
    IBaseValidator<TypeDto> validator)
    : ITypeHelper
{
    public async Task<TypeDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery<Type>().ById(id);

        return await
            repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<TypeDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery<Type>().ByName(name);

        return await
            repository.FindAsync(query);
    }

    public async Task<IEnumerable<TypeDto>> GetAllAsync()
    {
        return await 
            repository.GetAllAsync();
    }

    public async Task<Result<TypeDto>> CreateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await 
            repository.GetAllAsync();

        validator
            .WithExistingIds(existing.Select(t => t.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(t => t.Name));
        
        var validationResult = await validator.ValidateAsync(entity);

        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<TypeDto>();
            
            return Result<TypeDto>.Failure(errors);
        }
        
        var result = await repository.AddAsync(entity);

        if (result.IsFailure)
        {
            return Result<TypeDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<Result<TypeDto>> UpdateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<TypeDto>();
            
            return Result<TypeDto>.Failure(errors);
        }

        var result = await repository.UpdateAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<TypeDto>.Failure(result.Errors);
        }
     
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery<Type>().ById(id);

        return await 
            repository.DeleteAsync(query);
    }
}