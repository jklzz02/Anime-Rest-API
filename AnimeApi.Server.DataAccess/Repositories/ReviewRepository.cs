using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

/// <summary>
/// Responsible for providing data operations related to <see cref="Review"/> entities.
/// </summary>
public class ReviewRepository : IReviewRepository
{
    private readonly AnimeDbContext _context;

    /// <inheritdoc />
    public Dictionary<string, string> ErrorMessages { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for accessing review data.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public ReviewRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByAnimeIdAsync(int id)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.Anime_Id == id)
            .OrderByDescending(r => r.Created_At)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByAnimeTitleAsync(string title)
    {
        return await _context.Reviews
            .AsSplitQuery()
            .AsNoTracking()
            .Include(r => r.Anime)
            .Where(r => EF.Functions.Like(r.Anime.Name, $"%{title}"))
            .OrderByDescending(r => r.Created_At)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByUserIdAsync(int id)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.User_Id == id)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByUserEmailAsync(string email)
    {
        return await _context.Reviews
            .AsSplitQuery()
            .AsNoTracking()
            .Include(r => r.User)
            .Where(r => r.User.Email == email)
            .OrderByDescending(r => r.Created_At)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByDateAsync(DateTime date)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.Created_At.Date == date.Date)
            .OrderByDescending(r => r.Created_At)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetMostRecentByTimespanAsync(TimeSpan timespan)
    {
        var minDate = DateTime.UtcNow.Subtract(timespan);
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.Created_At >= minDate)
            .OrderByDescending(r => r.Created_At)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Review>> GetByMinScoreAsync(int minScore)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.Score >= minScore)
            .OrderByDescending(r => r.Score)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Review?> CreateAsync(Review review)
    {
        ArgumentNullException.ThrowIfNull(review, nameof(review));

        var entity = await GetByIdAsync(review.Id);
        var animeIds = await _context.Anime.Select(a => a.Id).ToListAsync();
        var userIds = await _context.Users.Select(u => u.Id).ToListAsync();
        
        if (entity is not null)
        {
            ErrorMessages.Add("id", $"Cannot add another review with id '{review.Id}'");
        }
        
        if (!userIds.Contains(review.User_Id))
        {
            ErrorMessages.Add("userId", $"Cannot add review with user id '{review.User_Id}'");
        }
        
        if (!animeIds.Contains(review.Anime_Id))
        {
            ErrorMessages.Add("animeId", $"Cannot add review with anime id '{review.Anime_Id}'");
        }

        if (ErrorMessages.Any())
        {
            return null;
        }
        
        var reviewEntity = _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(reviewEntity.Entity.Id);
    }
    
    /// <inheritdoc />
    public async Task<Review?> UpdateAsync(Review review)
    {
        ArgumentNullException.ThrowIfNull(review, nameof(review));

        var entity = await GetByIdAsync(review.Id);
        
        if (string.IsNullOrWhiteSpace(review.Content))
        {
            ErrorMessages.Add("content", "Content cannot be empty");
        }

        if (entity is null)
        {
            ErrorMessages.Add("id", $"there's no review with id {review.Id}");
        }

        if (ErrorMessages.Any())
        {
            return null;
        }
        
        entity!.Content = review.Content;
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return review;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            ErrorMessages.Add("id", $"there's no review with id {id}");
            return false;
        }
        _context.Reviews.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}