
using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.SpecHelpers;
public class ReviewQuery : QuerySpec<Review, ReviewQuery>
{
    public ReviewQuery ByPk(int id)
        => FilterBy(r =>  r.Id == id);

    public ReviewQuery ByUser(int userId)
        => FilterBy(r => r.User_Id == userId);

    public ReviewQuery ByUser(string username)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.User.Username, username));

    public ReviewQuery ByEmail(string email)
        => FilterBy(r => r.User.Email == email);

    public ReviewQuery ByAnime(int animeId)
        => FilterBy(r => r.Anime_Id == animeId);

    public ReviewQuery ByAnime(string title)
        => FilterBy(r => EF.Functions.TrigramsAreSimilar(r.Anime.Name, title));

    public ReviewQuery ByScoreRange(int min, int max)
        => FilterBy(r => r.Score >= min && r.Score <= max);

    public ReviewQuery ByDate(DateTime date)
        => FilterBy(r => r.Created_At.Date == date.Date);

    public ReviewQuery RecentByTimeSpan(TimeSpan span)
    {
        var now = DateTime.UtcNow;
        var range = now - span;

        FilterBy(r => r.Created_At <= now && r.Created_At >= range);

        return this;
    }
}
