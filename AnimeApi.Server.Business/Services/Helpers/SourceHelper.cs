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
    public async Task<SourceDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery<Source>().ById(id);
        return await
            repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<SourceDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery<Source>().ByName(name);

        return await
            repository.FindAsync(query);
    }

    public async Task<IEnumerable<SourceDto>> GetAllAsync()
    {
        return await 
            repository.GetAllAsync();
    }

    public async Task<Result<SourceDto>> CreateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await 
            repository.GetAllAsync();

        validator
            .WithExistingIds(existing.Select(p => p.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(p => p.Name));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }
        
        var result = await repository.AddAsync(entity);

        if (result.IsFailure)
        {
            return Result<SourceDto>.Failure(result.Errors);
        }
     
        return result;
    }

    public async Task<Result<SourceDto>> UpdateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }

        var result = await repository.UpdateAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<SourceDto>.Failure(result.Errors);
        }
     
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery<Source>().ById(id);
        return await
            repository.DeleteAsync(query);
    }
}