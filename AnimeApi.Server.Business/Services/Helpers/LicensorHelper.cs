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
    public async Task<LicensorDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery<Licensor>().ById(id);

        return await
            repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<LicensorDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery<Licensor>().ByName(name);
        
        return await
            repository.FindAsync(query);
    }
    
    public async Task<IEnumerable<LicensorDto>> GetAllAsync()
    {
        return await 
            repository.GetAllAsync();
    }

    public async Task<Result<LicensorDto>> CreateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var existing = await repository.GetAllAsync();

        validator
            .WithExistingIds(existing.Select(l => l.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(l => l.Name));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<LicensorDto>();

            return Result<LicensorDto>.Failure(errors);
        }
        
        var result = 
            await repository.AddAsync(entity);

        if (result.IsFailure)
        {
            return Result<LicensorDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<Result<LicensorDto>> UpdateAsync(LicensorDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<LicensorDto>();
            
            return Result<LicensorDto>.Failure(errors);
        }
        
        var result = await repository.UpdateAsync(entity);

        if (result.IsFailure)
        {
            return Result<LicensorDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery<Licensor>().ById(id);

        return await 
            repository.DeleteAsync(query);
    }
}