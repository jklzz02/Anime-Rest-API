using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.Business.Validators;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;

namespace AnimeApi.Server.Business.Services.Helpers;

public class GenreHelper(
    IRepository<Genre, GenreDto> repository,
    IBaseValidator<GenreDto> validator)
    : IGenreHelper
{
    private static BaseQuery<Genre> Query => new();
    
    public async Task<GenreDto?> GetByIdAsync(int id)
    {
        return await 
            repository.FindFirstOrDefaultAsync(Query.ById(id));
    }

    public async Task<IEnumerable<GenreDto>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return await
            repository.FindAsync(Query
                .ByName(name)
                .SortByName()
                .TieBreaker());
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        return await 
            repository.FindAsync(Query
                .SortByName()
                .TieBreaker());
    }

    public async Task<Result<GenreDto>> CreateAsync(GenreDto entity)
    {
      
        ArgumentNullException.ThrowIfNull(entity);
        
        var existing = await 
            repository.GetAllAsync();

        validator
            .WithExistingIds(existing.Select(g => g.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(g => g.Name));
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<GenreDto>();
            
            return Result<GenreDto>.Failure(errors);
        }

        var result = await repository.AddAsync(entity);
        
        return result.IsFailure
            ? Result<GenreDto>.Failure(result.Errors)
            : result;
    }

    public async Task<Result<GenreDto>> UpdateAsync(GenreDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ToJsonKeyedErrors<GenreDto>();
            
            return Result<GenreDto>.Failure(errors);
        }

        var result = await repository.UpdateAsync(entity);
        
        return result.IsFailure
            ? Result<GenreDto>.Failure(result.Errors)
            : result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(Query.ById(id));
    }
}