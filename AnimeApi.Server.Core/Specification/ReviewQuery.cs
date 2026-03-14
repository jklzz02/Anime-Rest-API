using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Sorting;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.Specification;

public class ReviewQuery : Specification<Review, ReviewQuery>
{
    public ReviewQuery ByPk(int id)
        => FilterBy(r =>  r.Id == id);

    public ReviewQuery ByText(string textQuery)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.Content, textQuery));
    

    public ReviewQuery ByUser(int userId)
        => FilterBy(r => r.UserId == userId);

    public ReviewQuery ByUser(string userEmail)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.User.Username, userEmail));

    public ReviewQuery ByEmail(string email)
        => FilterBy(r => r.User.Email == email);

    public ReviewQuery ByAnime(int animeId)
        => FilterBy(r => r.AnimeId == animeId);

    public ReviewQuery ByAnime(string title)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.Anime.Name, title));

    public ReviewQuery ByDate(DateTime date)
        => FilterBy(r => r.CreatedAt.Date == date.Date);

    public ReviewQuery RecentByTimeSpan(TimeSpan span)
    {
        var now = DateTime.UtcNow.ToUniversalTime();
        var range = now - span;

        FilterBy(r => r.CreatedAt <= now && r.CreatedAt >= range);

        return this;
    }

    public ReviewQuery Title(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return this;
        }

        FilterBy(r => EF.Functions.TrigramsAreSimilar(title, r.Title));

        return this;
    }
    
    public ReviewQuery Anime(int? animeId, string? animeTitle)
    {
        if (!animeId.HasValue && string.IsNullOrWhiteSpace(animeTitle))
        {
            return this;
        }

        if (animeId.HasValue)
        {
            return ByAnime(animeId.Value);
        }

        return !string.IsNullOrWhiteSpace(animeTitle)
            ? ByAnime(animeTitle)
            : this;
    }

    public ReviewQuery User(int? userId, string? userName)
    {
        if (!userId.HasValue && string.IsNullOrWhiteSpace(userName))
        {
            return this;
        }

        if (userId.HasValue)
        {
            return ByUser(userId.Value);
        }

        return !string.IsNullOrWhiteSpace(userName)
            ? FilterBy(r => EF.Functions.TrigramsAreSimilar(userName, r.User.Username))
            : this;
    }

    public ReviewQuery ScoreRange(decimal? min, decimal? max)
    {
        if (min.HasValue)
        {
            FilterBy(r => r.Score >= min);
        }

        if (max.HasValue)
        {
            FilterBy(r => r.Score <= max);
        }
        
        return this;
    }
    public ReviewQuery DateRange(DateTime? from, DateTime? to)
    {
        if (from.HasValue)
        {
            FilterBy(r => r.CreatedAt >= from.Value);
        }

        if (to.HasValue)
        {
            FilterBy(r => r.CreatedAt <= to.Value);
        }

        return this;
    }

    public ReviewQuery Sorting(string? field, string? order)
    {
        if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(order))
        {
            return this;
        }

        if (!ReviewSortMap.Validate(field))
        {
            throw new ArgumentException(
                $"Invalid order by field. Choose among: ({string.Join(", ", ReviewSortMap.Fields)})");
        }

        if (!SortConstants.Directions.Contains(order.ToLower()))
        {
            throw new ArgumentException(
                $"Invalid sort order. Choose among: ({string.Join(", ", SortConstants.Directions)})");
        }

        var ascending = order.EqualsIgnoreCase(SortConstants.Ascending);
        
        SortBy(ReviewSortMap.Action(field, ascending));
        
        return this;
    }
    
    public ReviewQuery IncludeUser()
        => Include(q => q.Include(r => r.User));
    
    public ReviewQuery IncludeAnime()
        => Include(q => q.Include(r => r.Anime));
}
