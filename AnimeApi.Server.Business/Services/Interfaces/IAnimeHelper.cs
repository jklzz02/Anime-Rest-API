using System.Linq.Expressions;
using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.Business.Services.Helpers;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface IAnimeHelper
{
    public IEnumerable<string> ErrorMessages { get; }
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
    Task<IEnumerable<AnimeDto>> GetByEpisodesAsync(int episodes);
    Task<IEnumerable<AnimeDto>> GetByTypeAsync(string type);
    Task<AnimeDto?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);
    Task<bool> CreateAsync(AnimeDto entity);
    Task<bool> UpdateAsync(AnimeDto entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AnimeDto>>? SearchAsync(AnimeSearchParameters parameters);
}