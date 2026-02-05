using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;
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

    public ReviewQuery ByUser(string username)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.User.Username, username));

    public ReviewQuery ByEmail(string email)
        => FilterBy(r => r.User.Email == email);

    public ReviewQuery ByAnime(int animeId)
        => FilterBy(r => r.AnimeId == animeId);

    public ReviewQuery ByAnime(string title)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.Anime.Name, title));

    public ReviewQuery ByScoreRange(int min, int max)
        => FilterBy(r => r.Score >= min && r.Score <= max);

    public ReviewQuery ByDate(DateTime date)
        => FilterBy(r => r.CreatedAt.Date == date.Date);

    public ReviewQuery RecentByTimeSpan(TimeSpan span)
    {
        var now = DateTime.UtcNow.ToUniversalTime();
        var range = now - span;

        FilterBy(r => r.CreatedAt <= now && r.CreatedAt >= range);

        return this;
    }
    
    public ReviewQuery IncludeUser()
        => Include(q => q.Include(r => r.User));
    
    public ReviewQuery IncludeAnime()
        => Include(q => q.Include(r => r.Anime));
}
