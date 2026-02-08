using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Specification;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.Business.Services.Helpers;

public class TypeHelper(
    IRepository<Type, TypeDto> repository,
    IBaseValidator<TypeDto> validator)
    : ITypeHelper
{
    private static BaseQuery<Type> Query => new();
    
    public async Task<TypeDto?> GetByIdAsync(int id)
    {
        return await
            repository.FindFirstOrDefaultAsync(Query.ById(id));
    }

    public async Task<IEnumerable<TypeDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        return await
            repository.FindAsync(Query
                .ByName(name)
                .SortByName()
                .TieBreaker());
    }

    public async Task<IEnumerable<TypeDto>> GetAllAsync()
    {
        return await 
            repository
                .FindAsync(Query
                    .SortByName()
                    .TieBreaker());
    }

    public async Task<Result<TypeDto>> CreateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var types = await repository.GetAllAsync();
        
        var existing = types
            .Where(t => !string.IsNullOrEmpty(t.Name))
            .ToList();

        validator
            .WithExistingIds(existing.Select(t => t.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(t => t.Name!));
        
        var validationResult = await validator.ValidateAsync(entity);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<TypeDto>();
            
            return Result<TypeDto>.Failure(errors);
        }
        
        var result = await repository.AddAsync(entity);

        return result.IsFailure
            ? Result<TypeDto>.Failure(result.Errors)
            : result;
    }

    public async Task<Result<TypeDto>> UpdateAsync(TypeDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<TypeDto>();
            
            return Result<TypeDto>.Failure(errors);
        }

        var result = await repository.UpdateAsync(entity);
        
        return result.IsFailure
            ? Result<TypeDto>.Failure(result.Errors)
            : result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await 
            repository.DeleteAsync(Query.ById(id));
    }
}