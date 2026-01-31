using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services.Helpers;

public class LicensorHelper(
    IRepository<Licensor, LicensorDto> repository,
    IBaseValidator<LicensorDto> validator)
    : ILicensorHelper
{
    private static BaseQuery<Licensor> Query => new();
    
    public async Task<LicensorDto?> GetByIdAsync(int id)
    {
        return await
            repository.FindFirstOrDefaultAsync(Query.ById(id));
    }

    public async Task<IEnumerable<LicensorDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        return await
            repository.FindAsync(Query
                .ByName(name)
                .SortByName()
                .TieBreaker());
    }
    
    public async Task<IEnumerable<LicensorDto>> GetAllAsync()
    {
        return await 
            repository.FindAsync(Query
                .SortByName()
                .TieBreaker());
    }

    public async Task<Result<LicensorDto>> CreateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var licensors = await repository.GetAllAsync();
        
        var existing =  licensors
            .Where(l => !string.IsNullOrWhiteSpace(l.Name))
            .ToList();

        validator
            .WithExistingIds(existing.Select(l => l.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(l => l.Name!));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<LicensorDto>();

            return Result<LicensorDto>.Failure(errors);
        }
        
        var result = 
            await repository.AddAsync(entity);

        return result.IsFailure
            ? Result<LicensorDto>.Failure(result.Errors)
            : result;
    }

    public async Task<Result<LicensorDto>> UpdateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<LicensorDto>();
            
            return Result<LicensorDto>.Failure(errors);
        }
        
        var result = await repository.UpdateAsync(entity);

        return result.IsFailure
            ? Result<LicensorDto>.Failure(result.Errors)
            : result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await 
            repository.DeleteAsync(Query.ById(id));
    }
}