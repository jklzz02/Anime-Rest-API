using System.Linq.Expressions;
using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface IAnimeHelper
{
    Dictionary<string, string> ErrorMessages { get; }
    Task<AnimeDto?> GetByIdAsync(int id);
    Task<IEnumerable<AnimeDto>> GetAllAsync();
    Task<IEnumerable<AnimeDto>?> GetAllAsync(int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByNameAsync(string name, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByProducerAsync(int producerId, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByLicensorAsync(int licensorId, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByGenreAsync(int genreId, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetBySourceAsync(string source, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByEnglishNameAsync(string englishName, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByScoreAsync(int score, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByReleaseYearAsync(int year, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByEpisodesAsync(int episodes, int page, int size = 100);
    Task<IEnumerable<AnimeDto>?> GetByTypeAsync(string type, int page, int size = 100);
    Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<AnimeDto?> CreateAsync(AnimeDto entity);
    Task<AnimeDto?> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AnimeDto>?> SearchAsync(AnimeSearchParameters parameters, int page, int size = 100);
}