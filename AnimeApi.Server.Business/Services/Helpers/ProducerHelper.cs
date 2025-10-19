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

public class ProducerHelper : IProducerHelper
{
    private readonly IRepository<Producer, ProducerDto> _repository;
    IMapper<Producer, ProducerDto> _mapper;
    private readonly IBaseValidator<ProducerDto> _validator;
    public ProducerHelper(
        IRepository<Producer, ProducerDto> repository,
        IMapper<Producer, ProducerDto> mapper,
        IBaseValidator<ProducerDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<ProducerDto?> GetByIdAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .AsNoTracking()
            .ToQuerySpec<Producer>();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    public async Task<IEnumerable<ProducerDto>> GetByNameAsync(string name)
    {
        var query = new BaseQuery()
            .ByName(name)
            .AsNoTracking()
            .ToQuerySpec<Producer>();

        return await
            _repository.FindAsync(query);
    }

    public async Task<IEnumerable<ProducerDto>> GetAllAsync()
    {
        return await 
            _repository.GetAllAsync();
    }

    public async Task<Result<ProducerDto>> CreateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var existing = await 
            _repository.GetAllAsync();

        _validator
            .WithExistingIds(existing.Select(p => p.Id.GetValueOrDefault()))
            .WithExistingNames(existing.Select(p => p.Name));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var result = await _repository.AddAsync(_mapper.MapToEntity(entity));

        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<Result<ProducerDto>> UpdateAsync(ProducerDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ProducerDto>();
            
            return Result<ProducerDto>.Failure(errors);
        };
        
        var result = await _repository.UpdateAsync(_mapper.MapToEntity(entity));
        
        if (result.IsFailure)
        {
            return Result<ProducerDto>.Failure(result.Errors);
        }
        
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var query = new BaseQuery()
            .ById(id)
            .AsNoTracking()
            .ToQuerySpec<Producer>();

        return await
            _repository.DeleteAsync(query);
    }
}