using AnimeApi.Server.Business.Extensions;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Specification;
using FluentValidation;

namespace AnimeApi.Server.Business.Services.Helpers;

/// <summary>
/// Provides helper methods for performing operations related to reviews,
/// such as retrieving, creating, updating, and deleting reviews associated with anime, users, and other criteria.
/// </summary>
public class ReviewHelper(
    IRepository<Review, ReviewDto> repository,
    IValidator<ReviewDto> validator)
    : IReviewHelper
{
    /// <inheritdoc />
    public async Task<PaginatedResult<ReviewDto>> GetAllAsync(int page, int size)
    {
        var query = new ReviewQuery()
            .SortBy(r => r.CreatedAt, SortDirection.Desc)
            .Paginate(page, size);

        var count = await
            repository.CountAsync();
        
        var result = await
            repository.FindAsync(query);
        
        return new PaginatedResult<ReviewDto>(result, page,  size, count);
    }
    
    /// <inheritdoc />
    public async Task<PaginatedResult<ReviewDetails>> GetAllDetailedAsync(int page, int size)
    {
        var query = new ReviewQuery()
            .SortBy(r => r.CreatedAt, SortDirection.Desc)
            .Paginate(page, size);

        var count = await
            repository.CountAsync();
        
        var result = await
            repository.FindAsync<ReviewDetails>(query);
        
        return new PaginatedResult<ReviewDetails>(result, page,  size, count);
    }

    /// <inheritdoc />
    public async Task<ReviewDto?> GetByIdAsync(int id)
    {
        var query = new ReviewQuery().ByPk(id);

        return await
            repository.FindFirstOrDefaultAsync(query);
    }

    /// <inheritdoc />
    public async Task<ReviewDetails?> GetDetailedByIdAsync(int id)
    {
        var query = new ReviewQuery()
            .ByPk(id);
        
        return await
            repository
                .FindFirstOrDefaultAsync<ReviewDetails>(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByAnimeIdAsync(int animeId)
    {
        var query = new ReviewQuery().ByAnime(animeId);

        return await
            repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByTitleAsync(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);

        var query = new ReviewQuery().ByAnime(title);

        return await
            repository.FindAsync(query);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
    {
        var query = new ReviewQuery().ByUser(userId);

        return await
            repository.FindAsync<ReviewDto>(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDetails>> GetDetailedByUserIdAsync(int userId)
    {
        var query = new ReviewQuery().ByUser(userId);

        return await
            repository.FindAsync<ReviewDetails>(query);
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<ReviewDto>>> GetByTextSearchAsync(string text)
    {
        if (text.Length >= 200)
        {
            return Result<IEnumerable<ReviewDto>>.ValidationFailure("lenght", "cannot be greater than 200 characters");
        }
        
        var query = new ReviewQuery().ByText(text);
        var results = await
            repository.FindAsync(query);

        return Result<IEnumerable<ReviewDto>>.Success(results);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByUserEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);

        var query = new ReviewQuery().ByEmail(email);

        return await
            repository.FindAsync(query);
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetByDateAsync(DateTime date)
    {
        var query = new ReviewQuery().ByDate(date);

        return await
            repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ReviewDto>> GetMostRecentByTimeSpanAsync(TimeSpan timeSpan)
    {
        var query = new ReviewQuery()
            .RecentByTimeSpan(timeSpan)
            .SortBy([
                SortAction<Review>.Desc(r => r.CreatedAt),
                SortAction<Review>.Desc(r => r.Score)
            ]);

        return await
            repository.FindAsync(query);
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<ReviewDto>>> GetByScoreAsync(int minScore, int maxScore)
    {
        List<Error> errors = [];
        
        if (minScore >= maxScore)
        {
            errors.Add(
                Error.Validation(
                    nameof(minScore),
                    "The min score cannot be greater than or equal to the max score.")
                );
        }

        if (minScore <= 0)
        {
            errors.Add(
                Error.Validation(
                    nameof(minScore),
                    "The min score cannot be less than or equal to zero."));
        }

        if (maxScore > 10)
        {
            errors.Add(
                Error.Validation(
                    nameof(minScore),
                    "The max score cannot be greater than ten.")
                );
        }

        if (errors.Any())
        {
            return Result<IEnumerable<ReviewDto>>.Failure(errors);
        }
        
        var query = new ReviewQuery()
            .ByScoreRange(minScore, maxScore);
        
        var results = await
            repository.FindAsync(query);
        
        return Result<IEnumerable<ReviewDto>>.Success(results);
    }

    /// <inheritdoc />
    public async Task<Result<ReviewDto>> CreateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return Result<ReviewDto>.Failure(errors);
        }

        var result = await
            repository.AddAsync(entity);

        if (result.IsFailure)
        {
            return Result<ReviewDto>.Failure(result.Errors);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<ReviewDto>> UpdateAsync(ReviewDto entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var validationResult = await validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            List<Error> errors = validationResult.Errors
                .ToJsonKeyedErrors<ReviewDto>();
            
            return Result<ReviewDto>.Failure(errors);
        }
        
        var result = await repository
            .UpdateAsync(entity);
        
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
            repository.DeleteAsync(query);
    }
}