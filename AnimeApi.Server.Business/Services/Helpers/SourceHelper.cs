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

public class SourceHelper : ISourceHelper
{
    private readonly IRepository<Source, SourceDto> _repository;
    private readonly IMapper<Source, SourceDto> _mapper;
    private readonly IBaseValidator<SourceDto> _validator;
    public SourceHelper(
        IRepository<Source, SourceDto> repository,
        IMapper<Source, SourceDto> mapper,
        IBaseValidator<SourceDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<SourceDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .AsNoTracking()
            .ToQuerySpec<Source>();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<SourceDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery()
            .ByName(name)
            .AsNoTracking()
            .ToQuerySpec<Source>();

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<SourceDto>> GetAllAsync()
    {
        return await 
            _repository.GetAllAsync();
    }

    public async Task<Result<SourceDto>> CreateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var existing = await 
            _repository.GetAllAsync();

        _validator
            .WithExistingIds(existing.Select(p => p.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(p => p.Name));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }
        
        var result = await _repository.AddAsync(_mapper.MapToEntity(entity));

        if (result.IsFailure)
        {
            return Result<SourceDto>.Failure(result.Errors);
        }
     
        return result;
    }

    public async Task<Result<SourceDto>> UpdateAsync(SourceDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<SourceDto>();
            
            return Result<SourceDto>.Failure(errors);
        }

        var result = await _repository.UpdateAsync(_mapper.MapToEntity(entity));
        
        if (result.IsFailure)
        {
            return Result<SourceDto>.Failure(result.Errors);
        }
     
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .AsNoTracking()
            .ToQuerySpec<Source>();

        return await
            _repository.DeleteAsync(query);
    }
}