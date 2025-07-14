using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Business.Extensions.Mappers;
using AnimeApi.Server.Business.Objects.Dto;
using AnimeApi.Server.Business.Services.Interfaces;
using AnimeApi.Server.Business.Validators.Interfaces;
using AnimeApi.Server.DataAccess.Services.Interfaces;

namespace AnimeApi.Server.Business.Services.Helpers;

/// <summary>
/// Provides helper methods for performing operations related to reviews,
/// such as retrieving, creating, updating, and deleting reviews associated with anime, users, and other criteria.
/// </summary>
public class ReviewHelper : IReviewHelper
{
    private readonly IReviewRepository _repository;
    private readonly IReviewValidator _validator;
    
    /// <inheritdoc />
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewHelper"/> class.
    /// </summary>
    /// <param name="repository">The repository for accessing and managing review data.</param>
    /// <param name="validator">The validator for validating review data.</param>
    public ReviewHelper(IReviewRepository repository, IReviewValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> GetByIdAsync(int id)
    {
        var review = await _repository.GetByIdAsync(id);
        return review?.ToDto();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByAnimeIdAsync(int animeId)
    {
        var reviews = await _repository.GetByAnimeIdAsync(animeId);
        return reviews.ToDto();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByTitleAsync(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);

        var reviews = await _repository.GetByAnimeTitleAsync(title);
        return reviews.ToDto();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
    {
        var reviews = await _repository.GetByUserIdAsync(userId);
        return reviews.ToDto();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        
        var reviews = await _repository.GetByUserEmailAsync(email);
        return reviews.ToDto();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByDateAsync(DateTime date)
    {
        var reviews = await _repository.GetByDateAsync(date);
        return reviews.ToDto();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetMostRecentByTimeSpanAsync(TimeSpan timeSpan)
    {
        var reviews = await _repository.GetMostRecentByTimespanAsync(timeSpan);
        return reviews.ToDto();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByMinScoreAsync(int minScore)
    {
        var reviews = await _repository.GetByMinScoreAsync(minScore);
        return reviews.ToDto();
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> CreateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return null;
        }
        
        var createdEntity = await _repository
            .CreateAsync(entity.ToModel());

        if (createdEntity is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return createdEntity.ToDto();
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> UpdateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var validationResult = await _validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            ErrorMessages = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return null;
        }
        
        var updatedEntity = await _repository
            .UpdateAsync(entity.ToModel());
        
        if (updatedEntity is null)
        {
            ErrorMessages = _repository.ErrorMessages;
            return null;
        }
        
        return updatedEntity.ToDto();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}