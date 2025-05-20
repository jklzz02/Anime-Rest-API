using System.Linq.Expressions;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

public interface IAnimeRepository : IRepository<Anime>
{
    Task<IEnumerable<Anime>> GetAllAsync(int page, int size = 100);
    Task<IEnumerable<Anime>> GetByNameAsync(string name, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByEnglishNameAsync(string englishName, int page, int size = 100);
    Task<IEnumerable<Anime>> GetBySourceAsync(string source, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByTypeAsync(string type, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByScoreAsync(int score, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByReleaseYearAsync(int year, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByEpisodesAsync(int episodes, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByLicensorAsync(int licensorId, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByProducerAsync(int producerId, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByGenreAsync(int genreId, int page, int size = 100);
    Task<IEnumerable<Anime>> GetByConditionAsync(
        int page,
        int size = 100,
        IEnumerable<Expression<Func<Anime, bool>>>? filters = null);
    Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
}