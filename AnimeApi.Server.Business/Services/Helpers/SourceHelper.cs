using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services.Helpers;

public class SourceHelper(
    IRepository<Source, SourceDto> repository,
    IBaseValidator<SourceDto> validator)
    : ISourceHelper
{
    private static BaseQuery<Source> Query => new();
    
    public async Task<SourceDto?> GetByIdAsync(int id)
    {
        return await
            repository.FindFirstOrDefaultAsync(Query.ById(id));
    }

    public async Task<IEnumerable<SourceDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        return await
            repository.FindAsync(Query
                .ByName(name)
                .SortByName()
                .TieBreaker());
    }

    public async Task<IEnumerable<SourceDto>> GetAllAsync()
    {
        return await 
            repository.FindAsync(Query
                .SortByName()
                .TieBreaker());
    }

    public async Task<Result<SourceDto>> CreateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var sources = await repository.GetAllAsync();
        
        var existing = sources
            .Where(s => !string.IsNullOrWhiteSpace(s.Name))
            .ToList();

        validator
            .WithExistingIds(existing.Select(p => p.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(p => p.Name!));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }
        
        var result = await repository.AddAsync(entity);

        return result.IsFailure
            ? Result<SourceDto>.Failure(result.Errors)
            : result;
    }

    public async Task<Result<SourceDto>> UpdateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }

        var result = await repository.UpdateAsync(entity);
        
        return result.IsFailure
            ? Result<SourceDto>.Failure(result.Errors)
            : result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await
            repository.DeleteAsync(Query.ById(id));
    }
}