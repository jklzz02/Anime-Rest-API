using System.Linq.Expressions;
using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Model;

namespace AnimeApi.Server.Business.Service.Interfaces;

public interface IAnimeHelper
{
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<IEnumerable<AnimeDto>> GetByNameAsync(string name);
    Task<IEnumerable<AnimeDto>> GetByProducerAsync(int producerId);
    Task<IEnumerable<AnimeDto>> GetByLicensorAsync(int licensorId);
    Task<IEnumerable<AnimeDto>> GetByGenreAsync(int genreId);
    Task<IEnumerable<AnimeDto>> GetBySourceAsync(string source);
    Task<IEnumerable<AnimeDto>> GetByEnglishNameAsync(string englishName);
    Task<IEnumerable<AnimeDto>> GetByScoreAsync(int score);
    Task<IEnumerable<AnimeDto>> GetByReleaseYearAsync(int year);
    Task<IEnumerable<AnimeDto>> GetByTypeAsync(string type);
    Task<IEnumerable<AnimeDto>> GetByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<bool> CreateAsync(AnimeDto entity);
    Task<bool> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    /*Task<IEnumerable<AnimeDto>>? SearchAsync(
        string? name = null,
        int? producerId = null,
        int? licensorId = null,
        int? genreId = null,
        string? source = null,
        string? type = null,
        string? englishName = null,
        int? minScore = null,
        int? maxScore = null,
        int? minReleaseYear = null,
        int? maxReleaseYear = null
    );
    */
}