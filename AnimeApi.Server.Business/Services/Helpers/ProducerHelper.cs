using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
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
    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery<Producer>().ById(id);
        
        return await
            repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<ProducerDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery<Producer>().ByName(name);

        return await
            repository.FindAsync(query);
    }

    public async Task<IEnumerable<ProducerDto>> GetAllAsync()
    {
        return await 
            repository.GetAllAsync();
    }

    public async Task<Result<ProducerDto>> CreateAsync(ProducerDto entity)
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
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var result = await repository.AddAsync(entity);

        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<Result<ProducerDto>> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var result = await repository.UpdateAsync(entity);
        
        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery<Producer>().ById(id);

        return await
            repository.DeleteAsync(query);
    }
}