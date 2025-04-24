using System.Linq.Expressions;
using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IAnimeRepository : IRepository<Anime>
{
    Task<IEnumerable<Anime>> GetByNameAsync(string name);
    Task<IEnumerable<Anime>> GetByEnglishNameAsync(string englishName);
    Task<IEnumerable<Anime>> GetBySourceAsync(string source);
    Task<IEnumerable<Anime>> GetByTypeAsync(string type);
    Task<IEnumerable<Anime>> GetByScoreAsync(int score);
    Task<IEnumerable<Anime>> GetByReleaseYearAsync(int year);
    Task<IEnumerable<Anime>> GetByEpisodesAsync(int episodes);
    Task<IEnumerable<Anime>> GetByLicensorAsync(int licensorId);
    Task<IEnumerable<Anime>> GetByProducerAsync(int producerId);
    Task<IEnumerable<Anime>> GetByGenreAsync(int genreId);
    Task<IEnumerable<Anime>> GetByConditionAsync(IEnumerable<Expression<Func<Anime, bool>>> condition);
    Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
}