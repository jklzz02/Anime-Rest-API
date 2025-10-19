using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.SpecHelpers;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

/// <summary>
/// Provides helper methods for performing operations related to reviews,
/// such as retrieving, creating, updating, and deleting reviews associated with anime, users, and other criteria.
/// </summary>
public class ReviewHelper : IReviewHelper
{
    private readonly IRepository<Review, ReviewDto> _repository;
    private readonly IMapper<Review, ReviewDto> _mapper;
    private readonly IValidator<ReviewDto> _validator;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewHelper"/> class.
    /// </summary>
    /// <param name="repository">The repository for accessing and managing review data.</param>
    /// <param name="validator">The validator for validating review data.</param>
    public ReviewHelper(
        IRepository<Review, ReviewDto> repository,
        IMapper<Review, ReviewDto> mapper,
        IValidator<ReviewDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> GetByIdAsync(int id)
    {
        var query = new ReviewQuery()
            .ByPk(id)
            .AsNoTracking();

        return await
            _repository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByAnimeIdAsync(int animeId)
    {
        var query = new ReviewQuery()
            .ByAnime(animeId)
            .AsNoTracking();

        return await
            _repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByTitleAsync(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);

        var query = new ReviewQuery()
            .ByAnime(title)
            .AsNoTracking();

        return await
            _repository.FindAsync(query);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
    {
        var query = new ReviewQuery()
            .ByUser(userId)
            .AsNoTracking();

        return await 
            _repository.FindAsync(query);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);

        var query = new ReviewQuery()
            .ByEmail(email)
            .AsNoTracking();

        return await
            _repository.FindAsync(query);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByDateAsync(DateTime date)
    {
        var query = new ReviewQuery()
            .ByDate(date)
            .AsNoTracking();

        return await
            _repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetMostRecentByTimeSpanAsync(TimeSpan timeSpan)
    {
        var query = new ReviewQuery()
            .AsNoTracking()
            .RecentByTimeSpan(timeSpan)
            .SortBy([
                SortAction<Review>.Desc(r => r.Created_At),
                SortAction<Review>.Desc(r => r.Score)
            ]);

        return await
            _repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByMinScoreAsync(int minScore)
    {
        var query = new ReviewQuery()
            .ByScoreRange(minScore, 10)
            .AsNoTracking();

        return await 
            _repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<Result<ReviewDto>> CreateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return Result<ReviewDto>.Failure(errors);
        }

        var result = await
            _repository.AddAsync(_mapper.MapToEntity(entity));

        if (result.IsFailure)
        {
            return Result<ReviewDto>.Failure(result.Errors);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<ReviewDto>> UpdateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return Result<ReviewDto>.Failure(errors);
        }
        
        var result = await _repository
            .UpdateAsync(_mapper.MapToEntity(entity));
        
        if (result.IsFailure)
        {
            return Result<ReviewDto>.Failure(result.Errors);
        }
     
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var query = new ReviewQuery()
            .ByPk(id);

        return await
            _repository.DeleteAsync(query);
    }
}