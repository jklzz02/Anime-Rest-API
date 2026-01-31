using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services.Helpers;

public class ProducerHelper(
    IRepository<Producer, ProducerDto> repository,
    IBaseValidator<ProducerDto> validator)
    : IProducerHelper
{
    private static BaseQuery<Producer> Query => new();
    
    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        return await
            repository.FindFirstOrDefaultAsync(Query.ById(id));
    }

    public async Task<IEnumerable<ProducerDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        return await
            repository.FindAsync(Query
                .ByName(name)
                .SortByName()
                .TieBreaker());
    }

    public async Task<IEnumerable<ProducerDto>> GetAllAsync()
    {
        return await 
            repository.FindAsync(Query
                .SortByName()
                .TieBreaker());
    }

    public async Task<Result<ProducerDto>> CreateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var producers = await repository.GetAllAsync();
        
        var existing = producers
            .Where(e => !string.IsNullOrWhiteSpace(e.Name))
            .ToList();
        
        validator
            .WithExistingIds(existing.Select(p => p.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(p => p.Name!));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        }
        
        var result = await repository.AddAsync(entity);

        return result.IsFailure
            ? Result<ProducerDto>.Failure(result.Errors)
            : result;
    }

    public async Task<Result<ProducerDto>> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var result = await repository.UpdateAsync(entity);
        
        return result.IsFailure
            ? Result<ProducerDto>.Failure(result.Errors)
            : result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await
            repository.DeleteAsync(Query.ById(id));
    }
}